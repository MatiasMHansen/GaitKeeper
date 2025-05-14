// wwwroot/js/downloadHelper.js
window.downloadFileFromBytes = (fileName, bytes) => {
    const blob = new Blob([new Uint8Array(bytes)], { type: 'text/csv' });
    const link = document.createElement('a');
    link.href = window.URL.createObjectURL(blob);
    link.download = fileName;
    link.click();
    window.URL.revokeObjectURL(link.href);
};