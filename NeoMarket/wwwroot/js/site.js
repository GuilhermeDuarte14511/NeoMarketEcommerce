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

var pageLayoutPrincipal = document.getElementById('pageLayoutPrincipal');
if (pageLayoutPrincipal) {
    document.addEventListener("DOMContentLoaded", function () {
        let cartCount = document.getElementById("CartItemCount");

        // Função para obter o valor do cookie
        function getCookie(name) {
            let match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
            return match ? match[2] : "0"; // Retorna "0" se o cookie não existir
        }

        // Obtendo a quantidade do carrinho do cookie
        let count = getCookie("CartItemCount");

        // Verifica se o elemento existe antes de modificar
        if (cartCount) {
            // Atualiza o conteúdo do badge
            cartCount.textContent = count;

            // Esconde o badge se a quantidade for 0
            if (count === "0" || count === "") {
                cartCount.style.display = "none"; // Esconde se estiver vazio
            } else {
                cartCount.style.display = "inline-block"; // Exibe o badge
            }
        }
    });

}

$(document).ready(function () {
    $("#searchInput").on("input", function () {
        let query = $(this).val().trim();
        if (query.length < 2) {
            $("#searchResults").hide();
            return;
        }

        $.ajax({
            url: "/Product/Search",
            method: "GET",
            data: { term: query },
            success: function (data) {
                console.log("Dados retornados:", data); // LOG para verificar os IDs

                let resultsContainer = $("#searchResults");
                resultsContainer.empty();

                if (data.length === 0) {
                    resultsContainer.append('<div class="search-item">Nenhum resultado encontrado</div>');
                } else {
                    data.forEach(item => {
                        console.log("ID do Produto:", item.id); // LOG para depuração

                        resultsContainer.append(`
                            <div class="search-item" data-id="${item.id}">
                                <img src="${item.imageUrl}" alt="${item.name}">
                                <div class="search-info">
                                    <strong>${item.name}</strong>
                                    <span class="price">R$ ${item.price.toFixed(2)}</span>
                                </div>
                            </div>
                        `);
                    });
                }

                resultsContainer.show();
            },
            error: function (xhr, status, error) {
                console.error("Erro na busca:", error);
            }
        });
    });

    // Captura do clique no item usando evento delegado
    $(document).on("click", ".search-item", function () {
        let productId = $(this).attr("data-id"); // Captura o ID corretamente
        productId = parseInt(productId); // Converte para número inteiro

        console.log("Produto clicado, ID capturado:", productId); // Log para depuração

        if (!productId || productId === 0 || isNaN(productId)) {
            console.error("Erro: ID do produto inválido.");
            return;
        }

        // Redirecionamento para a página de detalhes do produto
        window.location.href = `/Product/Details?productId=${productId}`;
    });

    // Esconder dropdown quando clicar fora
    $(document).click(function (e) {
        if (!$(e.target).closest("#searchInput, #searchResults").length) {
            $("#searchResults").hide();
        }
    });
});


// Funções específicas para a página de produtos
var ListProductPage = document.getElementById('ListProductPage');

if (ListProductPage) {

    // Obter os elementos de input de preço mínimo e máximo
    var minPriceInput = document.getElementById('minPrice');
    var maxPriceInput = document.getElementById('maxPrice');
    var sortBySelect = document.getElementById('sortBy');

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
        var sortBy = sortBySelect.value;

        var subCategorySlug = window.location.pathname.split("/").pop();

        // Validação dos preços
        if (minPrice > maxPrice && maxPrice > 0) {
            showToast('error', 'O preço mínimo não pode ser maior que o preço máximo.');
            return;
        }

        var filterRequest = {
            SubCategorySlug: subCategorySlug,
            Brands: selectedBrands,
            MaxPrice: maxPrice > 0 ? maxPrice : null,
            MinPrice: minPrice > 0 ? minPrice : null,
            MinRating: selectedRatings.length > 0 ? Math.max(...selectedRatings) : null,
            SortBy: sortBy
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

var detailsProductPage = document.getElementById('detailsProductPage');
if (detailsProductPage) {

    // Aplicar máscara de CEP e validar o formato
    document.getElementById('cepInput').addEventListener('input', function (e) {
        let value = e.target.value.replace(/\D/g, ''); // Remove caracteres não numéricos

        if (value.length > 8) {
            value = value.substring(0, 8); // Limita a 8 dígitos numéricos
        }

        if (value.length > 5) {
            value = value.replace(/^(\d{5})(\d{1,3})$/, '$1-$2'); // Formato 00000-000
        }

        e.target.value = value;
    });

    // Cálculo de Frete
    document.getElementById('calculateShippingButton').addEventListener('click', function () {
        const cep = document.getElementById('cepInput').value.trim();
        const productId = this.getAttribute('data-product-id');

        // Validação para apenas o formato correto de CEP (00000-000)
        const cepPattern = /^\d{5}-\d{3}$/;
        if (!cepPattern.test(cep)) {
            showToast('error', 'Por favor, insira um CEP válido no formato 00000-000.');
            return;
        }

        fetch(`/Product/CalculateShipping?productId=${productId}&cep=${cep}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Erro ao calcular o frete.');
                }
                return response.json();
            })
            .then(data => {
                const shippingOptions = document.getElementById('shippingOptions');
                shippingOptions.innerHTML = '<h6>Opções de Frete:</h6>';

                if (data.length > 0) {
                    data.forEach(option => {
                        if (!option.error) {
                            let optionHTML = `
                                <div class="card p-3 mb-2 shadow-sm d-flex flex-row align-items-center">
                                    <div class="me-3">
                                        <img src="${option.company?.picture}" alt="${option.company?.name}" class="img-fluid" style="width: 50px; height: 50px;">
                                    </div>
                                    <div>
                                        <h5 class="mb-1">${option.name}</h5>
                                        <p class="mb-1">Preço: <strong>${option.price ? parseFloat(option.price).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }) : 'R$ 0,00'}</strong></p>
                                        <p class="mb-1">Prazo de entrega: <strong>${option.delivery_time ? option.delivery_time + ' dias úteis' : 'Indisponível'}</strong></p>
                                        <p class="mb-1">Intervalo de entrega: <strong>${option.delivery_range ? option.delivery_range.min + ' a ' + option.delivery_range.max + ' dias úteis' : 'Indisponível'}</strong></p>
                                    </div>
                                </div>
                            `;
                            shippingOptions.innerHTML += optionHTML;
                        }
                    });

                    showToast('success', 'Cálculo de frete realizado com sucesso!');
                } else {
                    shippingOptions.innerHTML = '<p class="text-danger">Nenhuma opção de frete disponível.</p>';
                    showToast('error', 'Nenhuma opção de frete encontrada.');
                }
            })
            .catch(error => {
                console.error('Erro ao calcular frete:', error);
                showToast('error', 'Ocorreu um erro ao calcular o frete. Tente novamente mais tarde.');
            });
    });

    // Adicionar ao Carrinho
    document.getElementById('addToCartButton').addEventListener('click', function () {
        const productId = this.getAttribute('data-product-id');
        const quantity = 1;

        fetch('/Cart/AddToCart', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            },
            body: `productId=${productId}&quantity=${quantity}`
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    updateCartItemCount(data.itemCount);
                    showToast('success', 'Produto adicionado ao carrinho!');
                } else {
                    showToast('error', 'Falha ao adicionar produto ao carrinho.');
                }
            })
            .catch(error => {
                console.error('Erro ao adicionar produto ao carrinho:', error);
                showToast('error', 'Ocorreu um erro ao adicionar o produto ao carrinho.');
            });
    });

    // Função para atualizar o contador de itens no carrinho
    function updateCartItemCount(count) {
        const cartItemCount = document.getElementById('cartItemCount');
        if (cartItemCount) {
            cartItemCount.textContent = count;
        }
    }

}

// Variável para identificar a página do carrinho
var CartItemsDetailsPage = document.getElementById('CartItemsDetailsPage');

if (CartItemsDetailsPage) {

    // Aplicar máscara de CEP e validação de formato
    document.getElementById('cepInput').addEventListener('input', function (e) {
        let value = e.target.value.replace(/\D/g, ''); // Remove caracteres não numéricos

        if (value.length > 8) {
            value = value.substring(0, 8); // Limita a 8 dígitos numéricos
        }

        if (value.length > 5) {
            value = value.replace(/^(\d{5})(\d{1,3})$/, '$1-$2'); // Formato 00000-000
        }

        e.target.value = value;
    });

    // Calcular o frete ao clicar no botão
    document.getElementById('calculateShippingButton').addEventListener('click', function () {
        const cep = document.getElementById('cepInput').value.trim();
        const productIds = this.getAttribute('data-product-ids').split(',');

        const cepPattern = /^\d{5}-\d{3}$/;
        if (!cepPattern.test(cep)) {
            showToast('error', 'Por favor, insira um CEP válido no formato 00000-000.');
            return;
        }

        $('#freightOptionsContainer').html('Carregando opções de frete...');

        // Primeiro: Calcula o frete
        productIds.forEach(productId => {
            fetch(`/Cart/CalculateShipping?productId=${productId}&cep=${cep}`)
                .then(response => response.json())
                .then(data => {
                    let optionsHtml = '<h6>Opções de Frete:</h6>';
                    let hasValidOptions = false;

                    data.forEach(option => {
                        if (option.price && !option.error) {
                            hasValidOptions = true;
                            optionsHtml += `
                                <div class="form-check">
                                    <input class="form-check-input shipping-option" type="radio" 
                                           name="shippingOption" value="${option.price}" 
                                           data-delivery="${option.delivery_time}" 
                                           id="shipping-${option.name}">
                                    <label class="form-check-label" for="shipping-${option.name}">
                                        ${option.name} - ${parseFloat(option.price).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })} 
                                        (${option.delivery_time} dias úteis)
                                    </label>
                                </div>`;
                        }
                    });

                    if (!hasValidOptions) {
                        optionsHtml += '<p class="text-danger">Nenhuma opção de frete disponível.</p>';
                    }

                    $('#freightOptionsContainer').html(optionsHtml);

                    document.querySelectorAll('.shipping-option').forEach(option => {
                        option.addEventListener('change', updateTotalPrice);
                    });

                    showToast('success', 'Opções de frete carregadas com sucesso!');

                    // Segundo: Busca o endereço via API ViaCEP
                    fetch(`https://viacep.com.br/ws/${cep}/json/`)
                        .then(response => response.json())
                        .then(addressData => {
                            if (addressData.erro) {
                                showToast('error', 'CEP não encontrado. Verifique o CEP e tente novamente.');
                                return;
                            }

                            const fullAddress = `${addressData.logradouro || 'Rua não disponível'}, ${addressData.bairro || 'Bairro não disponível'}, ${addressData.localidade} - ${addressData.uf}`;

                            const addressElement = document.getElementById('addressText');
                            const addressInfoElement = document.getElementById('addressContainer');

                            if (addressElement && addressInfoElement) {
                                addressElement.innerText = fullAddress;
                                addressInfoElement.classList.remove('d-none');
                            } else {
                                console.warn('Elemento de endereço não encontrado no DOM.');
                            }
                        })
                        .catch(error => {
                            console.error('Erro ao buscar informações do CEP:', error);
                            showToast('error', 'Erro ao buscar informações do CEP.');
                        });
                })
                .catch(error => {
                    console.error('Erro ao buscar opções de frete:', error);
                    $('#freightOptionsContainer').html('<p class="text-danger">Erro ao buscar opções de frete.</p>');
                    showToast('error', 'Erro ao carregar as opções de frete.');
                });
        });
    });

    // Atualiza o preço total ao selecionar uma opção de frete
    function updateTotalPrice() {
        const selectedOption = document.querySelector('input[name="shippingOption"]:checked');
        const shippingCost = selectedOption ? parseFloat(selectedOption.value) : 0;
        const productTotal = parseFloat(document.getElementById('totalPrice').dataset.productTotal);
        const totalPrice = productTotal + shippingCost;

        document.getElementById('shippingCost').innerText = shippingCost.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
        document.getElementById('totalPrice').innerText = totalPrice.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
    }

    // Atualiza a quantidade de um item no carrinho
    function updateQuantity(productId, change) {
        $.post('/Cart/AddToCart', { productId: productId, quantity: change }, function (data) {
            if (data.success) {
                location.reload();
            } else {
                showToast('error', 'Não foi possível atualizar o carrinho.');
            }
        });
    }

    // Remove um item do carrinho
    function removeFromCart(productId) {
        $.post('/Cart/RemoveFromCart', { productId: productId }, function (data) {
            if (data.success) {
                updateCartItemCount(data.itemCount);
                location.reload();
            } else {
                showToast('error', 'Não foi possível remover o item do carrinho.');
            }
        });
    }

    // Atualiza o contador de itens no carrinho e salva no cookie
    function updateCartItemCount(count) {
        const cartItemCount = document.getElementById('cartItemCount');
        if (cartItemCount) {
            cartItemCount.textContent = count;
        }
        document.cookie = `CartItemCount=${count}; path=/; max-age=604800`; // 7 dias
    }
}

var loginAccountPage = document.getElementById('loginContainer');

if (loginAccountPage) {
    document.addEventListener("DOMContentLoaded", function () {
        const loginForm = document.getElementById("loginForm");
        const loginButton = document.getElementById("loginButton");
        const togglePassword = document.getElementById("togglePassword");
        const passwordInput = document.getElementById("password");

        // Alternar visualização da senha
        togglePassword.addEventListener("click", function () {
            const type = passwordInput.getAttribute("type") === "password" ? "text" : "password";
            passwordInput.setAttribute("type", type);
            this.innerHTML = type === "password" ? '<i class="bi bi-eye-fill text-muted"></i>' : '<i class="bi bi-eye-slash-fill text-primary"></i>';
        });

        loginForm.addEventListener("submit", function (e) {
            e.preventDefault();

            const formData = new FormData(loginForm);
            const loginData = {
                Email: formData.get("Email"),
                Password: formData.get("Password")
            };

            loginButton.disabled = true;
            loginButton.innerHTML = `
        <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Entrando...
    `;

            fetch("/Account/Login", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(loginData)
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        toastr.success(data.message, "Sucesso");

                        // Se `urlSlug` existir, redireciona para a página correta
                        const redirectUrl = data.urlSlug ? `/${data.urlSlug}` : "/";
                        setTimeout(() => { window.location.href = redirectUrl; }, 1000);
                    } else {
                        toastr.error(data.message, "Erro");
                        loginButton.disabled = false;
                        loginButton.innerHTML = "Entrar";
                    }
                })
                .catch(error => {
                    toastr.error("Ocorreu um erro inesperado. Tente novamente.", "Erro");
                    loginButton.disabled = false;
                    loginButton.innerHTML = "Entrar";
                });
        });

    });


}

