var dataTable;
var toastrTimeout = 2000;

//---Table Configuration Section---

$(document).ready(function () {
    dataTable = $("#ordersTable").DataTable({
        "ajax": {
            "url": "/Order/GetOrders",
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
                "render": $.fn.dataTable.render.moment("YYYY-MM-DDTHH:mm:ss", "DD/MM/YYYY")
            },
            {
                "targets": 3,
                "render": function () {
                    return `
                    <div class="handling-col">
                        <button type="button" class="table-icon edit-btn" data-toggle="modal" data-target="#updatingModal">
                            <i class="fas fa-edit"></i>
                        </button>
                        <button class="table-icon remove-btn">
                            <i class="fas fa-trash-alt"></i>
                        </button>
                    </div>`;
                },
                "orderable": false
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

//---Validation Section---

var oldPriceValue = "";
var oldDateValue = "";

//prevents connection to server if data hasn't changed
function updateValidation($form) {
    let newPriceValue = $form.find("input[name=Price]")[0].value;
    let newDateValue = $form.find("input[name=Date]")[0].value;
    if (oldDateValue == newDateValue && oldPriceValue == newPriceValue) {
        toastr.warning("Новые значения не отличаются от старых", "Неудачная операция", { timeOut: toastrTimeout });
        return false;
    }
    return true;
}

//palidations middleware
function validator($form) {
    let isValid = true;

    //data changes control
    if ($form.closest(".modal").attr("id") == "updatingModal") {
        isValid &= updateValidation($form);
    }

    //price value has to be positive
    if ($form.find("input[name=Price]")[0].value < 0) {
        $form.find("input[name=Price]")[0].classList.add("is-invalid");
        $form.find("span.field-validation-valid[data-valmsg-for='Price']").text("Cумма заказа не может быть отрицательной");
        isValid = false;
    }

    //"required" attribute behavior handling
    $form.find("input[required]").each(function () {
        if (!this.value) {
            let name = $(this).attr("name");
            this.classList.add("is-invalid");
            $form.find(`span.field-validation-valid[data-valmsg-for='${name}']`).text("Поле не может быть пустым");
            isValid = false;
        }
    });

    return isValid;
}

//reset all forms on modal closing
$(".modal").on("hidden.bs.modal", function () {
    $(this).find("input.form-control").val("").end();
    $(this).find("input.form-control").each(function () { this.classList.remove("is-invalid") });
});

//---Events Handling Section---

//date type format transforming
function formatDate(date) {
    date = new Date(date);
    let year = new Intl.DateTimeFormat("en", { year: "numeric" }).format(date);
    let month = new Intl.DateTimeFormat("en", { month: "2-digit" }).format(date);
    let day = new Intl.DateTimeFormat("en", { day: "2-digit" }).format(date);
    return `${year}-${month}-${day}`
}

//on edit button click handler: save old row data => set them to input
$(document).on("click", ".table-icon.edit-btn", function () {
    let order = dataTable.row($(this).closest("tr")).data();
    oldDateValue = formatDate(order.date);
    oldPriceValue = order.price.toString().replace(".", ",");

    $("#updatingModal #id").val(order.id);
    $("#updatingModal #date").val(oldDateValue);
    $("#updatingModal #price").val(oldPriceValue);
});

//on remove button click handler
$(document).on("click", ".table-icon.remove-btn", function () {
    let order = dataTable.row($(this).closest("tr")).data();
    order.price = order.price.toString().replace(".", ",");
    bootbox.confirm({
        message: "Вы действительно хотите удалить заказ?",
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
                    url: "/Order/RemoveOrder",
                    data: order,
                    success: function (resp) {
                        if (resp.status == 0) {
                            dataTable.ajax.reload();
                            toastr.success(resp.message, "Успешная операция", { timeOut: toastrTimeout });
                        }
                        else {
                            toastr.error(resp.message, "Неудачная операция", { timeOut: toastrTimeout });
                        }
                    },
                    error: function (err) {
                        toastr.error("Не удается установить соединение с сервером", "Ошибка соединения с сервером", { timeOut: toastrTimeout });
                    } 
                });
            }
        }
    });
});

//on form submit button click handler
$(".custom-form").submit(function (e) {
    e.preventDefault();
    var $form = $(this);
    $form.find("input.form-control").each(function () { this.classList.remove("is-invalid") });

    if (validator($form)) {
        $.ajax({
            type: "POST",
            url: this.action,
            data: $(this).serialize(),
            success: function (resp) {
                if (resp.status == 0) {
                    dataTable.ajax.reload();
                    toastr.success(resp.message, "Успешная операция", { timeOut: toastrTimeout });
                    $(".modal").modal("hide");
                }
                else {
                    toastr.error(resp.message, "Неудачная операция", { timeOut: toastrTimeout });
                    if (resp.modelState) {
                        for (field in resp.modelState) {
                            let errs = resp.modelState[field].errors;
                            if (errs.length != 0) {
                                $form.find(`input[name=${field}]`)[0].classList.add("is-invalid");
                                $form.find(`span.field-validation-valid[data-valmsg-for='${field}']`).text(errs[0].errorMessage);
                            }
                        }
                    }
                }
            },
            error: function (err) {
                toastr.error("Не удается установить соединение с сервером", "Ошибка соединения с сервером", { timeOut: toastrTimeout });
            } 
        });
    }
});