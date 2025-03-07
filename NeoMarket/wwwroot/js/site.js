// Inicialização de Toastr para Notificações
toastr.options = {
    closeButton: true,
    debug: false,
    newestOnTop: true,
    progressBar: true,
    positionClass: "toast-top-right",
    preventDuplicates: true,
    showDuration: "300",
    hideDuration: "1000",
    timeOut: "5000",
    extendedTimeOut: "1000",
    showEasing: "swing",
    hideEasing: "linear",
    showMethod: "fadeIn",
    hideMethod: "fadeOut"
};

// Exibe notificações de sucesso e erro
$(document).ready(function () {
    var successMessage = '@TempData["SuccessMessage"]';
    var errorMessage = '@TempData["ErrorMessage"]';

    if (successMessage) {
        toastr.success(successMessage);
    }

    if (errorMessage) {
        toastr.error(errorMessage);
    }
});

// Inicializa WOW.js para animações
new WOW().init();

// Ativa Tooltips do Bootstrap
$(function () {
    $('[data-bs-toggle="tooltip"]').tooltip();
});

// Máscaras de input com jQuery Mask
$(document).ready(function () {
    $('.cpf').mask('000.000.000-00', { reverse: true });
    $('.phone').mask('(00) 00000-0000');
    $('.zip-code').mask('00000-000');
    $('.price').mask('000.000.000.000.000,00', { reverse: true });
});

// Exemplo de uso de modal do Bootstrap
$('#exampleModal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget);
    var recipient = button.data('whatever');
    var modal = $(this);
    modal.find('.modal-body input').val(recipient);
});

// Exemplo de envio de formulário AJAX
$('#exampleForm').submit(function (e) {
    e.preventDefault();
    var form = $(this);
    $.ajax({
        type: form.attr('method'),
        url: form.attr('action'),
        data: form.serialize(),
        success: function (response) {
            toastr.success("Formulário enviado com sucesso!");
        },
        error: function () {
            toastr.error("Erro ao enviar o formulário.");
        }
    });
});

// Exemplo de animação em rolagem de página
$('a.scroll-to').on('click', function (e) {
    if (this.hash !== '') {
        e.preventDefault();
        var target = this.hash;
        $('html, body').animate({
            scrollTop: $(target).offset().top
        }, 800, function () {
            window.location.hash = target;
        });
    }
});

// Validação de formulários do Bootstrap
(() => {
    'use strict'
    const forms = document.querySelectorAll('.needs-validation')
    Array.from(forms).forEach(form => {
        form.addEventListener('submit', event => {
            if (!form.checkValidity()) {
                event.preventDefault()
                event.stopPropagation()
            }
            form.classList.add('was-validated')
        }, false)
    })
})();
