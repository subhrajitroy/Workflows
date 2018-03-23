using System;
using System.Threading.Tasks;

namespace Workflow
{
    class Program
    {
        static void  Main(string[] args)
        {
            var workflow = new WorkflowBuilder()
            .StartWith(new HelloActivity("Roy"))
            .Then(new HelloActivity("Mary"))
                .When(() => new Random().Next(1,10) % 2 == 0)
            .Then(new HelloActivity("Even John"))
            .Build();
            workflow.StartAsync().Wait();
        }
    }

    public class HelloActivity : TaskActivity
    {
        string Name {get;set;}
        public HelloActivity(string name) => Name = name;
        protected override Task RunAsync()
        {
            Console.WriteLine($"Hello {Name}");
            return Task.CompletedTask;
        }
    }
}
