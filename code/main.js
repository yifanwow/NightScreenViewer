const { app, BrowserWindow, Menu, ipcMain, shell } = require('electron');
const path = require('path'); // 确保正确引入了 path 模块
const { createMenu } = require('./menu');
const { enforceAspectRatio } = require('./resize');

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

  // 调用自定义的等比缩放函数
  //enforceAspectRatio(mainWindow);

  // 打开开发者工具
  //mainWindow.webContents.openDevTools();

  // 处理来自渲染器进程的消息
  ipcMain.on('message', (event, arg) => {
    console.log(arg); // 打印消息到控制台
  });
  ipcMain.on('go-back', () => {
    mainWindow.loadFile('index.html');
  });
}

app.whenReady().then(createWindow).catch(err => {
  console.error('Failed to create window:', err);
});

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit();
  }
});

app.on('activate', () => {
  if (BrowserWindow.getAllWindows().length === 0) {
    createWindow().catch(err => {
      console.error('Failed to create window on activate:', err);
    });
  }
});
