const { contextBridge, ipcRenderer } = require('electron');

contextBridge.exposeInMainWorld('electron', {
  sendMessage: (message) => ipcRenderer.send('message', message),
  receiveMessage: (callback) => ipcRenderer.on('backend-message', (event, data) => {
    console.log('PRELOAD: Received message from main process:', data); // 确认消息接收
    callback(data);
  }),
  goBack: () => ipcRenderer.send('go-back')
});
