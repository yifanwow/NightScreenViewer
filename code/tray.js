const { Tray, Menu, app } = require('electron');
const path = require('path');

function createTray(mainWindow) {
  const tray = new Tray(path.join(__dirname, 'IMG/logo/logo2.ico'));

  const contextMenu = Menu.buildFromTemplate([
    { label: 'Show App', click: () => { mainWindow.show(); } },
    { label: 'Quit', click: () => {
      app.isQuiting = true;
      app.quit();
    }}
  ]);

  tray.setToolTip('Night Screen Viewer');
  tray.setContextMenu(contextMenu);

  tray.on('click', () => {
    mainWindow.isVisible() ? mainWindow.hide() : mainWindow.show();
  });

  return tray;
}

module.exports = { createTray };
