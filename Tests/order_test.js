//POST - Create new order
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});

pm.test("Check if the user was added", function () {
    var responseBody = pm.response.json();
    pm.expect(responseBody).to.be.a('number');
});

//POST - Add order item
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});

//POST - Update order - checkout
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});

//POST - Update order - shipped
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});

//POST - Update order - received
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});

//DELETE - Delete order
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});