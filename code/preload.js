const { contextBridge, ipcRenderer } = require('electron');

contextBridge.exposeInMainWorld('electron', {
  sendMessage: (message) => ipcRenderer.send('message', message),
  goBack: () => ipcRenderer.send('go-back')
});
