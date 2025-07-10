window.downloadTableCsv = (tableId, filename) => {
    const table = document.getElementById(tableId);
    if (!table) return;
    const rows = Array.from(table.querySelectorAll('tr')).map(r =>
        Array.from(r.querySelectorAll('th,td')).map(c => '"' + c.innerText.replace(/"/g,'""') + '"').join(',')
    );
    const csv = rows.join('\n');
    const blob = new Blob([csv], { type: 'text/csv' });
    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.download = filename;
    link.click();
    URL.revokeObjectURL(link.href);
};

window.exportTableToPdf = (tableId, filename) => {
    const table = document.getElementById(tableId);
    if (!table || !window.jspdf) return;
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();
    const rows = Array.from(table.querySelectorAll('tr')).map(r =>
        Array.from(r.querySelectorAll('th,td')).map(c => c.innerText)
    );
    let y = 10;
    rows.forEach(row => {
        let x = 10;
        row.forEach(cell => {
            doc.text(cell, x, y);
            x += 40;
        });
        y += 10;
    });
    doc.save(filename);
};
