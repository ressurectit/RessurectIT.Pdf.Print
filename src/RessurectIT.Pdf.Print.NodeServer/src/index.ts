import {loadPackageDefinition, sendUnaryData, Server, ServerCredentials, ServerUnaryCall, UntypedHandleCall} from '@grpc/grpc-js';
import {loadSync} from '@grpc/proto-loader';
import {Status} from '@grpc/grpc-js/build/src/constants';
import {getDefaultPrinter, getPrinters, print} from 'pdf-to-printer';
import path from 'path';
import portfinder from 'portfinder';

import {AvailablePrinters} from './proto/AvailablePrinters';
import {Empty, Empty__Output} from './proto/Empty';
import {ProtoGrpcType} from './proto/pdfPrint';
import {Printer} from './proto/Printer';
import {PrintRequest} from './proto/PrintRequest';
import {PrintServiceHandlers} from './proto/PrintService';

const PROTO_PATH = path.join(__dirname, 'pdfPrint.proto');
const packageDefinition = loadSync(PROTO_PATH);
const printPdfProto: ProtoGrpcType = loadPackageDefinition(packageDefinition) as unknown as ProtoGrpcType;

class PrintService implements PrintServiceHandlers
{
    //######################### public properties - implementation of PrintServiceHandlers #########################

    /**
     * Handle for untyped calls
     */
    [name: string]: UntypedHandleCall;

    //######################### public methods - implementation of PrintServiceHandlers #########################
    
    /**
     * Gets default printer or null if no default printer
     * @param call - Call containing request data
     * @param callback - Callback used for returning data
     */
    public async GetDefaultPrinter(_call: ServerUnaryCall<Empty, Printer>, callback: sendUnaryData<Printer>): Promise<void>
    {
        try
        {
            const defPrinter = await getDefaultPrinter();
    
            callback(null, defPrinter);
        }
        catch(e)
        {
            callback(
            {
                code: Status.INTERNAL,
                message: `Getting default printer failed: ${e}`
            });

            return;
        }
    }

    /**
     * Gets array of all available printers
     * @param call - Call containing request data
     * @param callback - Callback used for returning data
     */
    public async GetPrinters(_call: ServerUnaryCall<Empty, AvailablePrinters>, callback: sendUnaryData<AvailablePrinters>): Promise<void>
    {
        try
        {
            const printers = await getPrinters();

            const result: AvailablePrinters =
            {
                printers
            };
    
            callback(null, result);
        }
        catch(e)
        {
            callback(
            {
                code: Status.INTERNAL,
                message: `Getting available printers failed: ${e}`
            });

            return;
        }
    }

    /**
     * Runs print of pdf document
     * @param call - Call containing request data
     * @param callback - Callback used for returning data
     */
    public async Print(call: ServerUnaryCall<PrintRequest, Empty__Output>, callback: sendUnaryData<Empty__Output>): Promise<void>
    {
        const pdf = call.request.pdfPath;

        if(!pdf)
        {
            callback(
            {
                code: Status.INVALID_ARGUMENT,
                message: 'Missing path to PDF'
            });

            return;
        }

        delete call.request.pdfPath;

        try
        {
            await print(pdf, call.request);
        }
        catch(e)
        {
            callback(
            {
                code: Status.INTERNAL,
                message: `Printing failed: ${e}`
            });

            return;
        }

        callback(null, {});
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
