import {getPrinters, getDefaultPrinter, print} from 'pdf-to-printer';

async function main()
{
    await print('Accessories International Printable Order Confirmation.pdf');

    const printers = await getPrinters();
    const defPrinter = await getDefaultPrinter();

    console.log(printers, defPrinter);
}

main();