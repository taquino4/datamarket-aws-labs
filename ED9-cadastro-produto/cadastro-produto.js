const formProduto = document.getElementById('formProduto');

formProduto.addEventListener('submit', async function (event) {
    event.preventDefault();

    const produto = {
        nome: document.getElementById('nome').value,
        preco: parseFloat(document.getElementById('preco').value),
        estoque: parseInt(document.getElementById('estoque').value)
    };

    try {
        const resposta = await fetch('/api/produtos', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(produto)
        });

        if (!resposta.ok) {
            throw new Error('Não foi possível cadastrar o produto.');
        }

        formProduto.reset();

        await carregarProdutos();

        alert('Produto cadastrado com sucesso!');
    }
    catch (erro) {
        console.error(erro);
        alert('Erro ao cadastrar produto.');
    }
});
