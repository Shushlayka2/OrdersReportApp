var dataTable;
$(document).ready(function () {
    dataTable = $("#ordersTable").DataTable({
        "ajax": {
            "url": "/Orders/GetOrders",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id" },
            { "data": "date" },
            { "data": "price" }
        ],
        "language": {
            "decimal": "",
            "emptyTable": "Нет данных",
            "info": "Заказы с _START_ по _END_ из _TOTAL_",
            "infoEmpty": "Заказы с 0 по 0 из 0",
            "infoFiltered": "(фильтрация по _MAX_ заказам)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Показать _MENU_ заказов",
            "loadingRecords": "Загрузка...",
            "processing": "Обработка...",
            "search": "Поиск:",
            "zeroRecords": "Не найдено подходящих заказов",
            "paginate": {
                "next": "След",
                "previous": "Пред"
            },
            "aria": {
                "sortAscending": ": сортировка по возрастанию",
                "sortDescending": ": сортировка по убыванию"
            }
        },
        "columnDefs": [
            {
                "targets": 1,
                "render": $.fn.dataTable.render.moment('YYYY-MM-DDTHH:mm:ss', 'DD/MM/YYYY')
            },
            {
                "targets": 3,
                "render": function () {
                    return `
                    <div class='handling_col'>
                        <button type="button" class="table-icon edit-btn" data-toggle="modal" data-target="#updatingModal">
                            <i class='fas fa-edit'></i>
                        </button>
                        <button class="table-icon remove-btn">
                            <i class='fas fa-trash-alt'></i>
                        </button>
                    </div>`;
                }
            },
            {
                "orderable": false, "targets": 3
            },
            {
                "width": "40%", "targets": 0,
                "width": "25%", "targets": 1,
                "width": "20%", "targets": 2,
                "width": "15%", "targets": 3
            }
        ]
    });
});

function formatDate(date) {
    date = new Date(date);
    let year = new Intl.DateTimeFormat('en', { year: 'numeric' }).format(date);
    let month = new Intl.DateTimeFormat('en', { month: '2-digit' }).format(date);
    let day = new Intl.DateTimeFormat('en', { day: '2-digit' }).format(date);
    return `${year}-${month}-${day}`
}

function on_succes(resp) {
    console.log("test");
    if (resp.status) {
        dataTable.ajax.reload();
        toastr.success(resp.message, 'Успешная операция', { timeOut: 2000 });
    }
    else {
        toastr.error(resp.message, 'Неудачная операция', { timeOut: 2000 });
    }
    $(".modal").modal("hide");
}

function on_error(resp) {
    toastr.error('Не удается установить соединение с сервером.', 'Ошибка соединения с сервером', { timeOut: 2000 });
}

$(document).on("click", ".table-icon.edit-btn", function () {
    let order = dataTable.row($(this).closest('tr')).data();
    $("#updatingModal #id").val(order.id);
    $("#updatingModal #date").val(formatDate(order.date));
    $("#updatingModal #price").val(order.price.toString().replace(".", ","));
})

$(document).on("click", ".table-icon.remove-btn", function () {
    let order = dataTable.row($(this).closest('tr')).data();
    order.price = order.price.toString().replace(".", ",");
    bootbox.confirm({
        message: 'Вы действительно хотите удалить заказ?',
        buttons: {
            cancel: {
                label: '<i class="fa fa-times"></i> Отмена'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> Подтвердить'
            }
        },
        callback: function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "/Orders/RemoveOrder",
                    data: order,
                    success: function (resp) {
                        if (resp.status) {
                            dataTable.ajax.reload();
                            toastr.success(resp.message, 'Успешная операция', { timeOut: 2000 });
                        }
                        else {
                            toastr.error(resp.message, 'Неудачная операция', { timeOut: 2000 });
                        }
                    },
                    error: on_error
                });
            }
        }
    });
});

$("#updatingForm").submit(function (e) {
    e.preventDefault();
    $.ajax({
        type: "POST",
        url: this.action,
        data: $(this).serialize(),
        success: on_succes,
        error: on_error
    });
});

$("#addingForm").submit(function (e) {
    e.preventDefault();
    $.ajax({
        type: "POST",
        url: this.action,
        data: $(this).serialize(),
        success: on_succes,
        error: on_error
    });
});