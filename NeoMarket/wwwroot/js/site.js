// 🟢 Função Global para exibir notificações Toast
function showToast(type, message) {
    if (type === 'success') {
        toastr.success(message);
    } else if (type === 'error') {
        toastr.error(message);
    } else if (type === 'info') {
        toastr.info(message);
    } else if (type === 'warning') {
        toastr.warning(message);
    } else {
        toastr.info(message);
    }
}

// 🟡 Configuração do Toastr
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

// 🔵 Exibe mensagens de sucesso e erro do TempData
$(document).ready(function () {
    var successMessage = '@TempData["SuccessMessage"]';
    var errorMessage = '@TempData["ErrorMessage"]';

    if (successMessage) {
        showToast('success', successMessage);
    }

    if (errorMessage) {
        showToast('error', errorMessage);
    }
});

// 🟠 Inicializa animações WOW.js
new WOW().init();

// 🟣 Ativa Tooltips do Bootstrap
$(function () {
    $('[data-bs-toggle="tooltip"]').tooltip();
});

// 🟤 Máscaras de input com jQuery Mask
$(document).ready(function () {
    $('.cpf').mask('000.000.000-00', { reverse: true });
    $('.phone').mask('(00) 00000-0000');
    $('.zip-code').mask('00000-000');
    $('.price').mask('000.000.000.000.000,00', { reverse: true });
});

// ⚪ Exemplo de envio de formulário AJAX com Toasts
$('#exampleForm').submit(function (e) {
    e.preventDefault();
    var form = $(this);
    $.ajax({
        type: form.attr('method'),
        url: form.attr('action'),
        data: form.serialize(),
        success: function () {
            showToast('success', 'Formulário enviado com sucesso!');
        },
        error: function () {
            showToast('error', 'Erro ao enviar o formulário.');
        }
    });
});

// Funções específicas para a página de produtos
var ListProductPage = document.getElementById('ListProductPage');

if (ListProductPage) {

    // Obter os elementos de input de preço mínimo e máximo
    var minPriceInput = document.getElementById('minPrice');
    var maxPriceInput = document.getElementById('maxPrice');

    // Evento de clique no botão de aplicar filtros
    document.getElementById('applyFiltersButton').addEventListener('click', function () {
        applyFilters();
    });

    // Função para aplicar os filtros
    function applyFilters() {
        var selectedBrands = Array.from(document.querySelectorAll('input[type=checkbox][id^=brand]:checked')).map(cb => cb.value);
        var selectedRatings = Array.from(document.querySelectorAll('input[type=checkbox][id^=rating]:checked')).map(cb => parseInt(cb.value));

        var minPrice = parseFloat(minPriceInput.value) || 0;
        var maxPrice = parseFloat(maxPriceInput.value) || 0;

        var subCategorySlug = window.location.pathname.split("/").pop();

        // Validação dos preços
        if (minPrice > maxPrice) {
            showToast('error', 'O preço mínimo não pode ser maior que o preço máximo.');
            return;
        }

        var filterRequest = {
            SubCategorySlug: subCategorySlug,
            Brands: selectedBrands,
            MaxPrice: maxPrice,
            MinPrice: minPrice,
            MinRating: selectedRatings.length > 0 ? Math.max(...selectedRatings) : null
        };

        fetch('/Product/GetFilteredProducts', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(filterRequest)
        })
            .then(response => {
                if (!response.ok) {
                    showToast('error', 'Erro ao buscar produtos filtrados.');
                    throw new Error('Erro na resposta do servidor.');
                }
                return response.json();
            })
            .then(data => {
                updateProductList(data);
                showToast('success', 'Produtos filtrados com sucesso!');
            })
            .catch(error => {
                showToast('error', 'Erro ao filtrar produtos: ' + error.message);
                console.error('Erro ao filtrar produtos:', error);
            });
    }

    // Função para atualizar a lista de produtos dinamicamente
    function updateProductList(products) {
        var productList = document.getElementById('productList');
        productList.innerHTML = '';

        if (!products.length) {
            productList.innerHTML = '<p>Nenhum produto disponível nesta subcategoria.</p>';
            showToast('info', 'Nenhum produto encontrado com os filtros aplicados.');
            return;
        }

        products.forEach(product => {
            var productHtml = `
                <div class="col-md-4 mb-4 product-item" data-price="${product.price}" data-brand="${product.brand}" data-rating="${product.rating}">
                    <div class="card shadow-sm">
                        <img src="${product.imageUrl}" class="card-img-top" alt="${product.name}">
                        <div class="card-body d-flex flex-column">
                            <h5 class="card-title">
                                <a href="#" class="text-decoration-none text-dark">${product.name}</a>
                            </h5>
                            <p class="text-muted">${product.description}</p>
                            <div class="text-warning mb-2">
                                ${renderStars(product.rating)}
                                <span class="text-muted">(${product.rating.toFixed(1)})</span>
                            </div>
                            <div class="mt-auto">
                                <p class="text-primary fs-5 fw-bold">${formatCurrency(product.price)}</p>
                                <a href="#" class="btn btn-outline-primary w-100">Detalhes</a>
                            </div>
                        </div>
                    </div>
                </div>`;
            productList.insertAdjacentHTML('beforeend', productHtml);
        });
    }

    // Função para renderizar as estrelas de avaliação
    function renderStars(rating) {
        var fullStars = Math.floor(rating);
        var halfStar = rating % 1 !== 0;
        var emptyStars = 5 - fullStars - (halfStar ? 1 : 0);

        return (
            '<i class="bi bi-star-fill"></i>'.repeat(fullStars) +
            (halfStar ? '<i class="bi bi-star-half"></i>' : '') +
            '<i class="bi bi-star"></i>'.repeat(emptyStars)
        );
    }

    // Função para formatar o preço no padrão brasileiro
    function formatCurrency(value) {
        return value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
    }
}
