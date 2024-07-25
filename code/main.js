const { app, BrowserWindow, Menu, ipcMain, Tray, shell } = require('electron');
const path = require('path');
const { createMenu } = require('./menu');
const { createTray } = require('./tray');

let mainWindow;
let tray = null;

function createWindow() {
  mainWindow = new BrowserWindow({
    width: 700,
    height: 500,
    minWidth: 350,
    minHeight: 250,
    icon: path.join(__dirname, 'IMG/logo/logo2.png'), // 设置窗口图标
    webPreferences: {
      preload: path.join(__dirname, 'preload.js'),
      nodeIntegration: false,
      contextIsolation: true
    }
  });

  mainWindow.loadFile('index.html');

  // 调用自定义的菜单创建函数
  createMenu(mainWindow);

  // 创建托盘图标
  tray = createTray(mainWindow);

  // 窗口关闭行为：最小化到托盘
  mainWindow.on('close', (event) => {
    if (!app.isQuiting) {
      event.preventDefault();
      mainWindow.hide();
    }
  });

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
