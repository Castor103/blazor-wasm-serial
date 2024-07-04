var jsSerialPort;
var jsSerialTextEncoder = new TextEncoder();

function jsSerialIsSupported() {
    return navigator.serial ? true : false;
}

function sayHello1() {

    // https://learn.microsoft.com/ko-kr/aspnet/core/blazor/javascript-interoperability/call-dotnet-from-javascript?view=aspnetcore-3.1
    //dotNetHelper.invokeMethodAsync({ASSEMBLY NAME}}, {ARGUMENTS});
    DotNet.invokeMethodAsync('FluentSample.Client', 'UpdateMessage', "test_arg");
}

async function jsSerialGetPort() {
    try {
        jsSerialPort = await navigator.serial.requestPort();
        return "Ok";
    } catch (ex) {
        console.log("ex: " + ex);
        
        if (ex.name == "NotFoundError") {
           return "NotFoundError";
        }
        else if (ex.name == "SecurityError") {
           return "SecurityError";
        }
        else if (ex.name == "AbortError") {
           return "AbortError";
        }
        else {
           return "Unknown";
        }
    }
}

async function jsSerialOpen(baudRate) {
    try {
        if(keepReading == false) {
            keepReading = true;

            console.log("baudRate : set " + baudRate);
            await jsSerialPort.open({ baudRate: baudRate });
            
            console.log("baudRate : ok");
            startListen();
           
            return "Ok";

        } else {
            console.log("Opened Port Exist");
            return "Unknown";
        }
    }
    catch (ex) {
        if (ex.name == "InvalidStateError") {
            return "InvalidStateError";
        }
        else if (ex.name == "NetworkError") {
            return "NetworkError";
        }
        else {
            return "Unknown";
        }
    } 
}

function jsSerialWriteText(text) {
    if(keepReading == true) {
        //if(writer != undefined) {
            let writer = jsSerialPort.writable.getWriter();
            writer.write(jsSerialTextEncoder.encode(text));
            writer.releaseLock();
        //}
    }
}

function jsSerialWriteBinary(data) {
    if(keepReading == true) {
        //if(writer != undefined) {
            //const data = new Uint8Array([104, 101, 108, 108, 111]); // hello
            let writer = jsSerialPort.writable.getWriter();
            writer.write(data);
            writer.releaseLock();
        //}
    }
}

//let writer;
let reader;
let keepReading = false;

async function startListen() {
    while (jsSerialPort.readable && keepReading) {
        reader = jsSerialPort.readable.getReader();
        //writer = jsSerialPort.writable.getWriter();
        try {
            while (true) {
                const { value, done } = await reader.read();
                if (keepReading == false || done) {
                    this.open = false;
                    break;
                }
                if(value) {
                    try
                    {
                        var result = String.fromCharCode.apply(null, value);
                        //console.log(":" + result);
                        DotNet.invokeMethodAsync('FluentSample.Client', 'UpdateMessage', result);
                    } catch (error) {
                        // TODO: Handle non-fatal read error.
                        console.log("startListenException: " + error);
                    }
                }
            }
        } catch (error) {
            // TODO: Handle non-fatal read error.
        } finally {
            await reader.releaseLock();
        }
    }

    
    //console.log("close1");
    //await reader.cancel();
    console.log("close2");
    await jsSerialPort.close();

    console.log("close3");
    this.close();
    //await jsSerialPort.close();

    console.log("close4");


}

async function jsSerialClose() {

    keepReading = false;

    if(reader != undefined) {

        // reader1 = jsSerialPort.readable.getReader();
        // await reader1.releaseLock();
        // await reader1.cancel();
        // await jsSerialPort.close();

        //reader = null;

    }

    console.log("jsSerialClose");

    //if(writer != undefined) {
    //    writer.cancel();
    //    writer = null;
    //}
}