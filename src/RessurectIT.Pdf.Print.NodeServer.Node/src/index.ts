import {getDefaultPrinter as getDefaultPrinterPdf, getPrinters as getPrintersPdf, print as printPdf} from 'pdf-to-printer';
import {Printer} from 'pdf-to-printer/dist/get-default-printer/get-default-printer';
import {PrintOptions} from 'pdf-to-printer/dist/print/print';
    
/**
 * Gets default printer or null if no default printer
 */
async function getDefaultPrinter(): Promise<Printer|null>
{
    return await getDefaultPrinterPdf();
}

/**
 * Gets array of all available printers
 */
async function getPrinters(): Promise<Printer[]>
{
    return await getPrintersPdf();
}

/**
 * Runs print of pdf document
 * @param pdf - Path to PDF file
 * @param options - Options for printing
 */
async function print(pdf: string, options?: PrintOptions): Promise<void>
{
    await printPdf(pdf, options);
}

module.exports = 
{
    getDefaultPrinter,
    getPrinters,
    print
};
