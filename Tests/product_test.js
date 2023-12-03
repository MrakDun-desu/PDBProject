//POST - Create new product
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});

pm.test("Check if the product was added", function () {
    pm.expect(responseBody).to.be.a('number');
});

//GET - Product by ID
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});

// Test to check if each product item has all the required keys and attributes
pm.test("Each product item has all the required keys and attributes", function () {
    var responseData = pm.response.json();
    pm.expect(responseData).to.have.property('id').to.be.a('number');
    pm.expect(responseData).to.have.property('name').to.be.a('string');
    pm.expect(responseData).to.have.property('price').to.be.a('number');
    pm.expect(responseData).to.have.property('stockCount').to.be.a('number');
    pm.expect(responseData).to.have.property('description').to.be.a('string');
    pm.expect(responseData).to.have.property('categories').to.be.a('array');
});

//POST - Update product
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});


//DELETE - Delete product
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});

