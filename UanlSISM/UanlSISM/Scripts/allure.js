var html = "";
html += "<!-- allure web solutions -->";
html += "<div class=\"allure\">";
html += "<a href=\"http:\/\/allurewebsolutions.com\"><img src=\"https:\/\/d1gug1ng40407.cloudfront.net\/uploads\/2017\/03\/Allure-Web-Solutions-Logo@2x.png\" width=\"100\"><\/a>";
html += "<a href=\"http:\/\/allurewebsolutions.com\/blog\/\">Learn more from our blog<\/a>";
html += "<\/div>";
html += "";
html += "<style>";
html += "  html, body {";
html += "    height: 100%;";
html += "  }";
html += "  #wrapper {";
html += "   min-height: 100%;";
html += "   margin-bottom: -100px;";
html += "  }";
html += "  #wrapper:after {";
html += "    content: '';";
html += "    display: block;";
html += "  }";
html += "  #wrapper:after, .allure {";
html += "    height: 80px;";
html += "  }";
html += "  .allure {";
html += "    width: 100%;";
html += "    background: rgba(255, 255, 255, 1);";
html += "    border-top: 2px solid rgba(42, 196, 234, 1);";
html += "    margin-top: 100vh";
html += "    padding: .5em;";
html += "    text-align: center;";
html += "    font-size: .6em;";
html += "  }";
html += "  a {";
html += "    display: block;";
html += "    text-decoration: none;";
html += "    text-transform: uppercase;";
html += "    margin-top: .25em;";
html += "    color: #333;";
html += "  }";
html += "  ";
html += "  a:hover {";
html += "    text-decoration: underline;";
html += "  }";
html += "<\/style>";

document.write(html);

// meta
var getUrl = window.location.host;
if (window.location.host != "s.codepen.io") {
    document.querySelector(".allure").style.display = "none";
}