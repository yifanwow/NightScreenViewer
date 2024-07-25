const { contextBridge, ipcRenderer } = require('electron');

contextBridge.exposeInMainWorld('electron', {
  sendMessage: (message) => ipcRenderer.send('message', message),
  receiveMessage: (callback) => ipcRenderer.on('backend-message', (event, data) => callback(data)),
  goBack: () => ipcRenderer.send('go-back')
});
