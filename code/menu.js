const { Menu, shell, app } = require('electron');

function createMenu(mainWindow) {
  const menuTemplate = [
    {
      label: 'Main Page  |',
      click: () => {
        mainWindow.loadFile('index.html');
      }
    },
    {
      label: 'Setting  |',
      click: () => {
        mainWindow.loadFile('settings.html');
      }
    },
    {
      label: 'Document  |',
      click: async () => {
        await shell.openExternal('https://github.com/yifanwow/NightScreenViewer');
      }
    },
    {
      label: 'Author  |',
      click: async () => {
        await shell.openExternal('https://yifanovo.info');
        mainWindow.loadFile('author.html');
      }
    },
    { type: 'separator' },
    {
      label: 'Exit',
      click: () => {
        app.isQuiting = true;
        app.quit();
      }
    }
  ];

  const menu = Menu.buildFromTemplate(menuTemplate);
  Menu.setApplicationMenu(menu);
}

module.exports = { createMenu };
