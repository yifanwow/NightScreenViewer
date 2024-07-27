const { contextBridge, ipcRenderer } = require('electron/renderer');

contextBridge.exposeInMainWorld('electron', {
  sendMessage: (message) => {
    return new Promise((resolve) => {
      ipcRenderer.once('message-received', () => resolve());
      ipcRenderer.send('message', message);
    });
  },
  receiveMessage: (callback) => ipcRenderer.on('backend-message', (_event, data) => callback(data)),
  goBack: () => ipcRenderer.send('go-back')
});
