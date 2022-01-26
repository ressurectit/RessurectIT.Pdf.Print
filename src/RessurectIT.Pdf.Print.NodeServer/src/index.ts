import {loadPackageDefinition, sendUnaryData, Server, ServerCredentials, ServerDuplexStream, ServerUnaryCall, UntypedHandleCall} from '@grpc/grpc-js';
import {loadSync} from '@grpc/proto-loader';
import {getDefaultPrinter, getPrinters} from 'pdf-to-printer';
import path from 'path';
import portfinder from 'portfinder';

import {AvailablePrinters} from './proto/AvailablePrinters';
import {Empty__Output} from './proto/Empty';
import {ProtoGrpcType} from './proto/pdfPrint';
import {Printer} from './proto/Printer';
import {PrintError} from './proto/PrintError';
import {PrintRequest__Output} from './proto/PrintRequest';
import {PrintServiceHandlers} from './proto/PrintService';

const PROTO_PATH = path.join(__dirname, 'pdfPrint.proto');
const packageDefinition = loadSync(PROTO_PATH);
const printPdfProto: ProtoGrpcType = loadPackageDefinition(packageDefinition) as unknown as ProtoGrpcType;


// /**
//  * Implements the SayHello RPC method.
//  */
// var sayHello: handleUnaryCall<HelloRequest, HelloReply> = function (call, callback) {
//   console.log(`volam ${JSON.stringify(call)}`)
//   callback(null, {message: `Hello ${call.request.name} Zarka`});
// }

// var sayHelloStreamReq: handleClientStreamingCall<HelloRequest, HelloReply> = function (call, callback)
// {
//   console.log(`volam stream req`, call);
//   console.log(`deadline`, call.getDeadline());

//   var data = [];

//   // call.on('close', () => console.log('zatvorene'));
//   call.on('end', () => 
//   { 
//     console.log('koniec')

//     callback(null, {message: data.join(', ')});
//   });
//   call.on('data', (req: HelloRequest) =>
//   {
//     console.log('data', req);
//     data.push(req.name);
//   })

// };

class PrintService implements PrintServiceHandlers
{
    //######################### public properties - implementation of PrintServiceHandlers #########################

    /**
     * Handle for untyped calls
     */
    [name: string]: UntypedHandleCall;

    //######################### public methods - implementation of PrintServiceHandlers #########################
    
    /**
     * 
     * @param call -
     * @param callback -
     */
    public async GetDefaultPrinter(_call: ServerUnaryCall<Empty__Output, Printer>, callback: sendUnaryData<Printer>): Promise<void>
    {
        const defPrinter = await getDefaultPrinter();

        callback(null, defPrinter);
    }

    /**
     * 
     * @param call -
     * @param callback -
     */
    public async GetPrinters(_call: ServerUnaryCall<Empty__Output, AvailablePrinters>, callback: sendUnaryData<AvailablePrinters>): Promise<void>
    {
        const printers = await getPrinters();

        const result: AvailablePrinters =
        {
            printers
        };

        callback(null, result);
    }

    /**
     * 
     * @param call -
     */
    public Print(_call: ServerDuplexStream<PrintRequest__Output, PrintError>)
    {

    }
}

const server = new Server();
server.addService(printPdfProto.PrintService.service, new PrintService());

portfinder.getPort({}, (err, port) =>
{
    if(err)
    {
        console.log(`PORT INIT ERROR: ${err}`);

        return;
    }

    if(!port)
    {
        console.log('PORT INIT ERROR: unable to find any port!');

        return;
    }

    server.bindAsync(`0.0.0.0:${port}`, ServerCredentials.createInsecure(), () =>
    {
        server.start();

        console.log(`PDF GRPC SERVER: running on '${port}'`);
    });
});
