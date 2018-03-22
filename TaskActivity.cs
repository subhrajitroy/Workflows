using System.Threading.Tasks;

namespace Workflow{

public interface ITaskActivity{
    void SetNext(ITaskActivity next);

    Task ExcuteAsync();
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
        await RunAsync();
        if(Next != null){
            await Next.ExcuteAsync();
        }
    }

    protected abstract Task RunAsync();
}

public class WorkflowBuilder{
    
    ITaskActivity root ;

    public WorkflowBuilder StartWith(ITaskActivity activity){
        root = activity;
        return this;
    }

    public WorkflowBuilder Then(ITaskActivity activity){
        root.SetNext(activity);
        return this;
    }

    public Workflow Build(){
        return new Workflow{Root = root};
    }
  
}

 public class Workflow{
     public ITaskActivity Root {get;set;}

     public async Task  StartAsync(){
        await Root.ExcuteAsync();
     }
 }

}