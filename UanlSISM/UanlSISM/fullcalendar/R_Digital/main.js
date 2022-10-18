function relogio() {
    let horas = document.getElementById("horas");
    let minutos = document.getElementById("minutos");
    let segundos = document.getElementById("segundos");
    let ampm = document.getElementById("am-pm");

    let h = new Date().getHours();
    let m = new Date().getMinutes();
    let s = new Date().getSeconds();
    var am = 'AM';


    // Converter 24hrs para 12hrs com indicador AM PM
    if (h > 12) {
        h = h - 12;
        var am = 'PM'
    }
    // Adionar mais um 0 nos numeros
    h = (h < 10) ? '0' + h : h;
    m = (m < 10) ? '0' + m : m;
    s = (s < 10) ? '0' + s : s;

    horas.innerHTML = h;
    minutos.innerHTML = m;
    segundos.innerHTML = s;
    ampm.innerHTML = am;
}

var intervalo = setInterval(relogio, 1000);