/// <reference path="~/Public/jquery.js"/>
function AddTiempoComida() {
    var value = $("#tiemposcomida").val();
    var container = $('<div></div>');
    var text = $("#tiemposcomida").find(':selected').text();
    var input = $("<input>", {
        type: 'hidden',
        value: value,
        name: 'tiemposdecomida'
    });
    $("#contenttiemposcomida").append(container.append(createRemoveButton($("#contenttiemposcomida"), container)).append(input).append("<b>" + text + "</b>"));
}
function GroupChange() {
    var groupid = $("#grupos").val();
    $.get('/Recomendation/GetFoods', { groupid: groupid }, function (data) {
        $("#alimentos").empty();
        $.each(data, function (i, item) {
            $("#alimentos").append("<option value=" + item.id + ">" + item.name + "</option>");
        });
    });
}
function AddAlimento() {
    var alimentoid = $("#alimentos").val();
    var index = $("#contenedoralimentos").children().length;
    $("#contenedoralimentos").append(createAlimento(alimentoid, index));
}
function createAlimento(alimentoid, index) {
    var container = $("<div></div>");
    var input = $("<input />").attr({
        name: "alimentos[" + index + "].Cantidad",
        type: "text"
    });
    //var description = $("<input>").attr({
    //    name: "alimentos[" + index + "].Descripcion",
    //    type: "text"
    //});
    container.append(createRemoveButton($("#contenedoralimentos"), container));
    container.append(input);
    container.append("<b>" + $("#alimentos").find(':selected').text() + "</b>");
    container.append($("<input />").attr({
        name: "alimentos[" + index + "].AlimentoID",
        type: "hidden",
        value: alimentoid
    }));
    //container.append(createRemoveButton($("#contenedoralimentos"), container)).append(input).append("<b>" + $("#alimentos").find(':selected').text() + "</b>").append(
    //        $("<input />").attr({
    //            name: "alimentos[" + index + "].AlimentoID",
    //            type: "hidden",
    //            value: alimentoid
    //        }));
    return container;
}
function createRemoveButton(supercontainer, container) {
    return $('<button>', {
        type: 'button',
        class: 'btn btn-default'
    }).click(function () {
        supercontainer.find(container).remove();
    }).append($('<img>').attr({
        src: "/Public/icons/delete.png",
        width: '10px'
    }));
}
function deleteElement(element) {
    var container = $(element).parent();
    $(element).parent().parent().find(container).remove();
}