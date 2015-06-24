using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bradaz.Clarizen.API;
using Xamasoft.JsonClassGenerator;
using Xamasoft.JsonClassGenerator.CodeWriters;

namespace PlainClarizen
{
    class Program
    {
        static void Main(string[] args)
        {
            RestClient client = new RestClient("garfbradaz", "Poohead26@");
            client.GetAllMetadataDescribeEntitiesAndGetAllFields(entityType.Task);

            var gen = Prepare();
            
            
        }

        private JsonClassGenerator Prepare(RestClient client)
        {
            if (client.Tasks.Count == 0)
            {

                return null;
            }


            if (edtMainClass.Text == string.Empty)
            {
                MessageBox.Show(this, "Please specify a main class name.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            var gen = new JsonClassGenerator();
            gen.Example = client.Tasks.ToString();
            gen.InternalVisibility = false;
            gen.CodeWriter = new CSharpCodeWriter();
            gen.ExplicitDeserialization = false;

            Add read line fpr namespace ;
            gen.Namespace = string.IsNullOrEmpty(edtNamespace.Text) ? null : edtNamespace.Text;
            gen.NoHelperClass = chkNoHelper.Checked;
            gen.SecondaryNamespace = string.Empty;
            gen.TargetFolder = edtTargetFolder.Text;
            gen.UseProperties = radProperties.Checked;
            gen.MainClass = edtMainClass.Text;
            gen.UsePascalCase = chkPascalCase.Checked;
            gen.UseNestedClasses = radNestedClasses.Checked;
            gen.ApplyObfuscationAttributes = chkApplyObfuscationAttributes.Checked;
            gen.SingleFile = chkSingleFile.Checked;
            gen.ExamplesInDocumentation = chkDocumentationExamples.Checked;
            return gen;
        }


    }
}
