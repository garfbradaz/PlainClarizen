using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bradaz.Clarizen.API;
using Xamasoft.JsonClassGenerator;
using Xamasoft.JsonClassGenerator.CodeWriters;
using System.Diagnostics;

namespace PlainClarizen
{
    class Program
    {
        static void Main(string[] args)
        {
            RestClient client = new RestClient("garfbradaz", "Poohead26@");
            client.GetAllMetadataDescribeEntitiesAndGetAllFields(entityType.Task);

            if (!string.IsNullOrWhiteSpace(client.ConvertedMetadata))
            {
                var gen = Prepare(client, entityType.Task);
                if (gen == null) return;

                try
                {
                    gen.GenerateClasses();

                }
                catch (Exception ex)
                {
                    Console.Write("Error writing class " + ex);
                }
            }
            Console.ReadLine();
        }

        static JsonClassGenerator Prepare(RestClient client,entityType type)
         
        {

            var gen = new JsonClassGenerator();
            gen.Example = client.ConvertedMetadata;
            gen.InternalVisibility = false;
            gen.CodeWriter = new CSharpCodeWriter();
            gen.ExplicitDeserialization = false;

            //Add read line fpr namespace ;
            var input = string.Empty;
            ReadInputFromUser(ref input," Please enter a Namespace ");

            gen.Namespace = "MainNameSpace.Main";
            gen.NoHelperClass = false;
            gen.SecondaryNamespace = "SecondNameSpace.Main";
            //gen.UseNamespaces = true;
            gen.TargetFolder = @"D:\test data\PlainClarizen";
            gen.UseProperties = true;

            switch(type)
            {
                case entityType.Task:
                    gen.MainClass = "ClarizenTask";
                        break;
                default:
                        break;
            }
            
            gen.UsePascalCase = true;
            gen.UseNestedClasses = false;
            gen.ApplyObfuscationAttributes = false;
            gen.SingleFile = true;
            gen.ExamplesInDocumentation = false;
            return gen;
        }

        static void ReadInputFromUser(ref string input, string displayMessage, string[] validInputStrings = null)
        {
            //string yn = string.Empty;
            bool validated = false;
            string[] ynArray = {"Y","N","y","n"};

            while (true)
            {
                Console.WriteLine(displayMessage);
                input = Console.ReadLine();

                if(validInputStrings != null)
                {
                    if (validInputStrings.Length > 0)
                    {
                        for (int i = 0; i < validInputStrings.Length; i++)
                        {
                            if (validInputStrings[i] == input)
                            {
                                validated = true;
                                Console.WriteLine("You have entered the following " + input);
                                string yn = string.Empty;
                                ReadInputFromUser(ref yn, "Is that OK? ");
                                if (yn == "Y" || yn == "y")
                                {
                                    Debug.WriteLine("Y or N entered " + input);
                                    break;
                                }
                            }
                        }
                    }


                    if(!validated)
                    {
                        Console.WriteLine("What you entered was incorrect " + input + ". You need to input");
                        for(int i = 0; i < validInputStrings.Length; i++)
                        {
                            Console.WriteLine(i + " " + validInputStrings[i]);
                        }

                    }
                    else
                    {
                        break;
                    }
                }

                if (input == "exit" || validInputStrings == null)
                {
                    break;
                }

            }

        }
    }
}
        


