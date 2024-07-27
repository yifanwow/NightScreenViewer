document.addEventListener('DOMContentLoaded', () => {
    const authorApp = document.getElementById('authorApp');
  
    authorApp.innerHTML = `
      <div class="container">
        <h1>Yifan Yu</h1>
        <p>Always believe that tech should make life better.<p>
        <button class="button" id="backButton">Back</button>
      </div>
    `;
  
    document.getElementById('backButton').addEventListener('click', () => {
      window.electron.goBack();
    });
  });
  