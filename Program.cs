using System;
using System.Data.SqlClient;
using System.Text;

//PRÉ-REQUISITOS:
// Install Visual Studio Code
// Install .NET Core SDK (Preview 2 version)
// Install NuGet Package Manager from the Visual Studio Code Extension Marketplace
// Install C# extension from Visual Studio Code Extension Marketplace

//PASSOS:
// 1-Launch Visual Studio Code
// 2-Open your project folder OU crie um novo projeto com o comando: dotnet new console 
// 3-Launch VS Code Command Palette by pressing F1 or Ctrl+Shift+P or Menu Bar > View > Command Palette
// 4-In Command Palette box, type nu
// 5-Click on NuGet Package Manager: Add Package
// 6-Enter package filter e.g. system.data (Enter your assembly reference here)
// 7-Press Enter
// 8-Click on System.Data.SqlClient
// 9-The following prompt pops up
// 10-Click on Restore OU dotnet restore no terminal

namespace SQLServer_ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Conectando-se ao SQL Server para demonstrar as operaçõesde CRUD Create(Criação), Read(Leitura), Update(Atualização) e Delete(Exclusão).");

                // Build connection string (Construindo a string de conexão)
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                //builder.DataSource = "DIOGO-DESKTOP\\SQLEXPRESS";   // (update me) (localhost) nome do servidor do Sql Server no seu computador
                builder.DataSource = "DESKTOP-E0A397V";
                builder.UserID = "joao";              // (update me) Usuário padrão do Sql Server (sa) é reamente um usuário que existe no Sql Server por padrão. Você também pode criar e usar um outro qualquer.
                builder.Password = "123456";      // (update me) senha do usuário do Sql Server
                builder.InitialCatalog = "master"; // banco de dados padrão que existe no Sql Server só para iniciar a conexão

                // Connect to SQL (Conectando-se ao Sql Server)
                Console.Write("Conectando-se ao SQL Server ... ");
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    Console.WriteLine("Done (Feito).");

                    // Create a sample database (Criando banco de dados de exemplo)
                    Console.Write("Descartando e criando banco de dados 'SampleDB' ... ");
                    String sql = "DROP DATABASE IF EXISTS [SampleDB]; CREATE DATABASE [SampleDB]";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Feito.");
                    }

                    // Create a Table and insert some sample data (Criando uma tabela e inserindo alguns dados de exemplo)
                    Console.Write("Criando tabela de com dados de exemplo, pressione qualquer tecla para continuar...");
                    Console.ReadKey(true);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("USE SampleDB; ");
                    sb.Append("CREATE TABLE Funcionarios ( ");
                    sb.Append(" Codigo INT IDENTITY(1,1) NOT NULL PRIMARY KEY, ");
                    sb.Append(" Nome NVARCHAR(50), ");
                    sb.Append(" Local NVARCHAR(50) ");
                    sb.Append("); ");
                    sb.Append("INSERT INTO Funcionarios (Nome, Local) VALUES ");
                    sb.Append("(N'Joao', N'Australia'), ");
                    sb.Append("(N'Maria', N'India'), ");
                    sb.Append("(N'Jose', N'Alemanha'); ");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Feito.");
                    }

                    // Demonstração de inserção
                    Console.Write("Inserindo uma nova linha na tabela, pressione qualquer tecla para continuar...");
                    Console.ReadKey(true);
                    sb.Clear();
                    sb.Append("INSERT Funcionarios (Nome, Local) ");
                    sb.Append("VALUES (@nome, @local);");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@nome", "Juca");
                        command.Parameters.AddWithValue("@local", "Estados Unidos");
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " linhas(s) inseridas");
                    }

                    // Demonstração de atualização
                    String userToUpdate = "Maria";
                    Console.Write("Atualizando 'Local' do usuário '" + userToUpdate + "', pressione qualquer tecla para continuar...");
                    Console.ReadKey(true);
                    sb.Clear();
                    sb.Append("UPDATE Funcionarios SET Local = N'Estados Unidos' WHERE Nome = @nome");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@nome", userToUpdate);
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " linha(s) atualizada(s)");
                    }

                    // Demonstração de exclusão
                    String userToDelete = "Jose";
                    Console.Write("Excluindo usuário '" + userToDelete + "', pressione qualquer tecla para continuar...");
                    Console.ReadKey(true);
                    sb.Clear();
                    sb.Append("DELETE FROM Funcionarios WHERE Nome = @nome;");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@nome", userToDelete);
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " linha(s) deletad(s)");
                    }

                    // Demonstração de leitura
                    Console.WriteLine("Lendo dados da tabela, pressione qualquer tecla para continuar...");
                    Console.ReadKey(true);
                    sql = "SELECT Codigo, Nome, Local FROM Funcionarios;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0} {1} {2}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("Tudo feito. Pressione qualquer tecla para continuar...");
            Console.ReadKey(true);
        }
    }
}