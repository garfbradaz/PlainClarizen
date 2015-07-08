using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bradaz.Clarizen.API;
using Xamasoft.JsonClassGenerator;
using Xamasoft.JsonClassGenerator.CodeWriters;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Security;

namespace PlainClarizen
{
    class Program
    {
        static void Main(string[] args)
        {
            bool repeat = true;
            while (repeat)
            {

                var mainNameSpace = string.Empty;
                var folder = string.Empty;
                var userName = string.Empty;
                var passWord = string.Empty;
                string howManyEntities = string.Empty;
                int count = 0;
                string countEntities = "N";
                string[] yn = { "Y", "N", "y", "n" };
                bool limitEntities = false;

                ReadInputFromUser(ref userName, "Please enter your Clarizen Username");
                ReadInputFromUserSecretly(ref passWord, "Please enter your Clarizen Password");
                ReadInputFromUser(ref mainNameSpace, "Please enter a Main Namespace ");
                ReadInputFromUser(ref countEntities, "Parse all Clarizen Entities? Y/N ", yn);


                if (countEntities == "N" || countEntities == "n") limitEntities = true;
                if (limitEntities == true)
                {
                    ReadInputFromUser(ref howManyEntities, "How many Entities to parse?");
                    try
                    {
                        count = Convert.ToInt32(howManyEntities);
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("Input string is not a sequence of digits.");
                    }
                    catch (OverflowException e)
                    {
                        Console.WriteLine("The number cannot fit in an Int32.");
                    }
                }


                //TODO add logic to test this is a correct drive/folder.
                ReadInputFromUser(ref folder, "Please enter the folder location to save your code files ");


                ///need to change your password.
                RestClient client = new RestClient(userName, passWord);



                Console.WriteLine("Fetching data from Clarizen...");
                if (client.ListEntities != null)
                {
                    int counting = 0;
                    foreach (string s in client.ListEntities)
                    {
                        counting++;
                        client.GetAllMetadataDescribeEntitiesAndGetAllFields(s);
                        Console.Write(".");
                        if (counting > count && limitEntities == true) break;
                    }
                    Console.WriteLine();
                }



                if (client.ConvertedMetadata != null)
                {


                    Console.WriteLine("Creating Classes...");
                    foreach (KeyValuePair<string, JToken> pair in client.ConvertedMetadataWithoutCustomFields)
                    {

                        Console.Write(".");
                        var gen = Prepare(pair.Value, pair.Key, folder, mainNameSpace);
                        if (gen == null) return;

                        try
                        {
                            gen.GenerateClasses();

                        }
                        catch (Exception ex)
                        {
                            Console.Write("Error writing class " + ex + pair.Key + " " + pair.Value);
                        }

                    }
                }

 

                Console.WriteLine();
                Console.Write("Finished! - code should be ready in folder " + folder + " ");
                Console.WriteLine();
                string go = string.Empty;
                ReadInputFromUser(ref go, "Continue? ");
                if (go == "Y" || go == "y")
                {
                    repeat = true;
                }
                else
                {
                    repeat = false;
                }


            }
        }


        static JsonClassGenerator Prepare(JToken json, string type, string folder,
            string mainNameSpace = "MainNameSpace.Main")
        {

            var gen = new JsonClassGenerator();
            gen.Example = json.ToString();
            gen.InternalVisibility = false;
            gen.CodeWriter = new CSharpCodeWriter();
            gen.ExplicitDeserialization = false;

            gen.Namespace = mainNameSpace;
            gen.NoHelperClass = false;

            gen.TargetFolder = @folder;
            gen.UseProperties = true;
            gen.MainClass = type;

            gen.UsePascalCase = true;
            gen.UseNestedClasses = false;
            gen.ApplyObfuscationAttributes = false;
            gen.SingleFile = true;
            gen.ExamplesInDocumentation = false;
            gen.InheritedClassOrInterface = "ClarizenEntity";
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


        static void ReadInputFromUserSecretly(ref string input, string displayMessage)
        {
            
            Console.WriteLine(displayMessage);
            ConsoleKeyInfo key;
            // Backspace Should Not Work

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    input += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                    {
                        input = input.Substring(0, (input.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            
        }


    }
}
        


