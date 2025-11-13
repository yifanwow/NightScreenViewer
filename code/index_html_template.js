document.addEventListener("DOMContentLoaded", () => {
  const app = document.getElementById("app");

  app.innerHTML = `
    <div class="container">
      <h1>Night Screen Viewer</h1>
      <div class="toggle-container">
         <div class="checkbox-wrapper-5">
          <div class="check">
            <input id="toggle" type="checkbox">
            <label for="toggle"></label>
          </div>
        </div>
        <button id="autoOn" class="button button-on">Auto On</button>
      </div>
      <div class="radio-wrapper-15">
  </div>

<div class="checkbox-wrapper-47">
  <input type="checkbox" name="cb" id="cb-theater" />
  <label for="cb-theater">Theater Mode</label>
</div>
<div class="checkbox-wrapper-47">
  <input type="checkbox" name="cb" id="cb-47" />
  <label for="cb-47">Mirror Mode</label>
</div>

      <div class="input-container">
        <div id="decrease" class="button1">
          <svg viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"> 
            <g id="SVGRepo_iconCarrier">
              <path d="M12 2C6.49 2 2 6.49 2 12C2 17.51 6.49 22 12 22C17.51 22 22 17.51 22 12C22 6.49 17.51 2 12 2ZM15.92 12.75H7.92C7.51 12.75 7.17 12.41 7.17 12C7.17 11.59 7.51 11.25 7.92 11.25H15.92C16.33 11.25 16.67 11.59 16.67 12C16.67 12.41 16.34 12.75 15.92 12.75Z" fill="currentColor"></path>
            </g>
          </svg>
        </div>
        <input type="range" id="opacity" min="0" max="100" value="50">
        <div id="increase" class="button1">
          <svg viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
            <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
            <g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g>
            <g id="SVGRepo_iconCarrier">
              <path fill-rule="evenodd" clip-rule="evenodd" d="M13 9C13 8.44772 12.5523 8 12 8C11.4477 8 11 8.44772 11 9V11H9C8.44772 11 8 11.4477 8 12C8 12.5523 8.44772 13 9 13H11V15C11 15.5523 11.4477 16 12 16C12.5523 16 13 15.5523 13 15V13H15C15.5523 13 16 12.5523 16 12C16 11.4477 15.5523 11 15 11H13V9ZM2 12C2 6.47715 6.47715 2 12 2C17.5228 2 22 6.47715 22 12C22 17.5228 17.5228 22 12 22C6.47715 22 2 17.5228 2 12Z" fill="currentColor"></path>
            </g>
          </svg>
        </div>
      </div>
      <label for="opacity">Opacity: <span id="opacityValue">50%</span></label>
      
    </div>
    <img src="./IMG/logo/nsv_logo_img.png" class="bottom-right-logo">
  `;
});
