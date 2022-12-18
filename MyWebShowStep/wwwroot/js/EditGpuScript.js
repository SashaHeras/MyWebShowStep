var obj = {};

function ReadFilds() {
    obj.vendor = document.getElementById("vendor").value;
    obj.chip = document.getElementById("chip").value;
    obj.ram = document.getElementById("ram").value;
    obj.memory = document.getElementById("memory").value;
    obj.productId = document.getElementById("prdId").value;
    obj.id = document.getElementById("dataId").value;

    console.log(obj);
}