namespace BaristaLabs.Espresso.Core.v1
{
    using BaristaLabs.Espresso.Common;
    using Nancy;
    using System.Threading.Tasks;

    public class EvalModule : NancyModule
    {
        public EvalModule()
        {
            Get["/{appName}/Engine/Eval/{virtualScriptPath}", runAsync: true] = async (_, token) =>
            {
                //Obtain the app from the 'appstore' implmentation.

                //Using the virtual file system associated with the app, obtain the script to execute.

                //Obtain a script engine instance of the appropriate type from the pool.
                //var engine = new V8ScriptEngine();

                //Evaluate the script on the script engine instance.
                await Task.Run(() =>
                {
                });

                //Based on the value returned (and if there were changes to the virtual response object) set the response.

                //Return the response.
                return "asdf";
            };
        }
    }
}
