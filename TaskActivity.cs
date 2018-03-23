using System;
using System.Threading.Tasks;

namespace Workflow{

public interface ITaskActivity{
    void SetNext(ITaskActivity next);

    Task ExcuteAsync();

    Func<bool> ExecutionCondition {get;set;}
}



public abstract class TaskActivity : ITaskActivity {
    
    private ITaskActivity Next {get;set;}

    public void SetNext(ITaskActivity next){
        if(Next == null){
            Next = next;
            return;
        }
        Next.SetNext(next);
    }

    public async Task ExcuteAsync(){
        if(ExecutionCondition()) await RunAsync();
        if(Next != null){
            await Next.ExcuteAsync();
        }
    }

    public Func<bool> ExecutionCondition{get;set;} = () => true;

    protected abstract Task RunAsync();
}

public class WorkflowBuilder{
    
    ITaskActivity root ;
    ITaskActivity current ;

    public WorkflowBuilder StartWith(ITaskActivity activity){
        root = activity;
        current = root;
        return this;
    }

    public WorkflowBuilder Then(ITaskActivity activity){
        current.SetNext(activity);
        current = activity;
        return this;
    }

    public WorkflowBuilder When(Func<bool> when){
        current.ExecutionCondition = when;
        return this;
    }

    public Workflow Build(){
        return new Workflow{Root = root};
    }
  
}

public class LastResult{
    public object Data{get;set;}
}

 public class Workflow{
     public ITaskActivity Root {get;set;}

     public async Task  StartAsync(){
        await Root.ExcuteAsync();
     }
 }

 

}