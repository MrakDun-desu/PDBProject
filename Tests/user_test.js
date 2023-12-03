//POST - Create new user
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});

pm.test("Check if the user was added", function () {
    var responseBody = pm.response.json();
    pm.expect(responseBody).to.be.a('number');
});

//GET - User by ID

// Test for the existence of keys and their types in the response

pm.test("Response object has the required keys and correct types", function () {
    var responseJSON = pm.response.json();
    pm.expect(responseJSON).to.have.property('id').to.be.a('number');
    pm.expect(responseJSON).to.have.property('name').to.be.a('string');
    pm.expect(responseJSON).to.have.property('email').to.be.a('string');
    pm.expect(responseJSON).to.have.property('address').to.be.a('string');

    if (responseJSON.orders && responseJSON.orders.length > 0) {
        responseJSON.orders.forEach((order) => {
            pm.expect(order).to.have.property('id').to.be.a('number');
            pm.expect(order).to.have.property('state').to.be.a('number');
            pm.expect(order).to.have.property('orderedDate').to.be.a('string');
            pm.expect(order).to.have.property('shippedDate').to.be.a('string');
            pm.expect(order).to.have.property('receivedDate').to.be.a('string');

            order.orderItems.forEach((item) => {
                pm.expect(item).to.have.property('productId').to.be.a('number');
                pm.expect(item).to.have.property('productCount').to.be.a('number');
            });
        });
    }
});


//PUT - Update user
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});


//DELETER - Delete user
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});