const fs = require('fs'),
      path = require('path');

const protoFile = fs.readFileSync(path.join(__dirname, '..', 'pdfPrint.proto')).toString();

fs.writeFileSync(path.join(__dirname, 'src', 'proto', 'pdfPrint.proto.ts'), 
`export default \`${protoFile}\``);
