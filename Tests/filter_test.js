//GET - Filter price range

pm.test('Status code is 200', function () {
    pm.response.to.have.status(200);
})

pm.test("Each product has price between the values of the params in URL", function () {
    var urlParams = pm.request.url.getPath().match(/(\d+)/g);
    var minPrice = parseInt(urlParams[0]);
    var maxPrice = parseInt(urlParams[1]);

    pm.response.json().forEach(function(product) {
        pm.expect(product.price).to.be.within(minPrice, maxPrice);
    });
});

//GET - Filter category
pm.test('Response status code is 200', function () {
    pm.response.to.have.status(200);
});

pm.test("Each product has the category from the URL", function () {
    var categoryInURL = pm.request.url.path.join('/').split('/').pop(); // Extracting category from the URL
    var products = pm.response.json();
    var foundMatchingCategory = false;
    products.forEach(function(product) {
        if (product.categories.includes(categoryInURL)) {
            foundMatchingCategory = true;
        }
    });
    pm.expect(foundMatchingCategory).to.be.true;
});