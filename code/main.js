const { app, BrowserWindow, Menu, ipcMain, shell } = require('electron');
const path = require('path');
const { createMenu } = require('./menu');

function createWindow() {
  const mainWindow = new BrowserWindow({
    width: 700,
    height: 500,
    minWidth: 350,
    minHeight: 250,
    webPreferences: {
      preload: path.join(__dirname, 'preload.js'),
      nodeIntegration: false,
      contextIsolation: true
    }
  });

  mainWindow.loadFile('index.html');

  // 调用自定义的菜单创建函数
  createMenu(mainWindow);

  // 处理来自渲染器进程的消息
  ipcMain.on('message', (event, arg) => {
    console.log(arg); // 打印消息到控制台
  });

  ipcMain.on('go-back', () => {
    mainWindow.loadFile('index.html');
  });
}

app.whenReady().then(createWindow);

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit();
  }
});

app.on('activate', () => {
  if (BrowserWindow.getAllWindows().length === 0) {
    createWindow();
  }
});
