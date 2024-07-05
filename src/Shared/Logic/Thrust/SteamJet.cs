using System.Text;

namespace FluentSample.Shared.Logic.Thrust;

public class SteamJet : ILogicModule
{
    public bool IsOn { get; private set; }

    private int Temp1 { get; set; } = 0;

    public async Task Loop()
    {
        try
        {
            Console.WriteLine($"loop {this.GetType().Name} - start");

            //init();

            while(IsOn)
            {
                //process();

                if(!IsOn)
                {
                    break;
                }

                await Task.Delay(1000);
            }

            Console.WriteLine($"loop {this.GetType().Name} - end");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    public async Task<bool> Start()
    {
        await Task.Delay(1);

        if(IsOn)
        {
            Console.WriteLine($"start {this.GetType().Name} - fail, device on");

            return false;
        }
        else
        {
            Console.WriteLine($"start {this.GetType().Name} - ok");
            this.IsOn = true;

            #pragma warning disable 4014
            this.Loop();
            #pragma warning restore 4014

            return true;
        }
    }

    public async Task<bool> Stop()
    {
        await Task.Delay(1);
        
        if(IsOn)
        {
            Console.WriteLine($"stop {this.GetType().Name} - ok");
            
            this.IsOn = false;

            return true;
        }
        else
        {
            Console.WriteLine($"stop {this.GetType().Name} - fail, device off");

            return false;
        }
    }

    public async Task Reset()
    {
        await Task.Delay(1);

        if(IsOn)
        {
            Console.WriteLine($"reset {this.GetType().Name} - ok");
        }
        else
        {
            Console.WriteLine($"reset {this.GetType().Name} - fail, device off");
        }
    }

    public async Task SetManual(string cmd)
    {
        await Task.Delay(1);
    }
   
    public async Task<byte[]> GetResponse(byte[] cmd)
    {
        string result = string.Empty;

        string cmdString = Encoding.UTF8.GetString(cmd);

        Console.WriteLine($"GetResponseString {this.GetType().Name} - [{cmdString}]");

        if(cmdString.Length > 0)
        {
            switch(cmdString)
            {
                case "ready?":
                    result = "ok";
                    break;
                case "temp?":
                    result = $"temp {Temp1}\n";
                    break;
                default:
                    break;
            }
        }

        await Task.Delay(1);

        if(result.Length > 0)
        {
            return Encoding.UTF8.GetBytes(result);;
        }
        else
        {
            return new byte[0];
        }
    }

    public async Task<byte[]> GetAsyncResponse()
    {
        await Task.Delay(1);
        return new byte[0];
    }

    public async Task<string> GetResponseString(string cmd)
    {
        string result = string.Empty;

        if(cmd.Length > 0)
        {
            switch(cmd)
            {
                case "ready?":
                    result = "ok";
                    break;
                case "temp?":
                    result = $"temp {Temp1}\n";
                    break;
                default:
                    break;
            }
        }

        await Task.Delay(1);

        return result;
    }

    public async Task<string> GetAsyncResponseString()
    {
        await Task.Delay(1);
        return "GetResponseString";
    }

}
