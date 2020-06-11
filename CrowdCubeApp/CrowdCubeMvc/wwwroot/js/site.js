// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$("#signup-form").submit(function (e) {
    $("input[type='submit']", this)
        .val("Please Wait...")
        .attr('disabled', 'disabled');

    actionMethod = "POST"
    if ($('#Role').val() == 'ProjectCreator') {
        actionUrl = "/user/addprojectcreator"
        sendData = {
            "Username": $('#Username').val(),
            "Password": $('#Password').val(),
            "FirstName": $('#FirstName').val(),
            "LastName": $('#LastName').val(),
            "DoB": $('#DoB').val(),
            "Email": $('#Email').val(),
            "Role": $('#Role').val()
        }
    } else {
        actionUrl = "/user/addbacker"
        sendData = {
            "Username": $('#Username').val(),
            "Password": $('#Password').val(),
            "FirstName": $('#FirstName').val(),
            "LastName": $('#LastName').val(),
            "DoB": $('#DoB').val(),
            "Email": $('#Email').val(),
            "Role": $('#Role').val()
        }
    }


    $.ajax({
        url: actionUrl,
        dataType: 'json',
        type: actionMethod,
        data: JSON.stringify(sendData),


        contentType: 'application/json',
        processData: false,
        success: function (data, textStatus, jQxhr) {
            role = data["role"]
            if (role == 'ProjectCreator') {
                userId = data["id"]
                window.open("/HOME/ProjectCreatorIndex?userId=" + userId, "_self")
            } else {
                userId = data["id"]
                window.open("/HOME/BackerIndex?userId=" + userId, "_self")
            }
        },
        error: function (jqXhr, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
    e.preventDefault();
});


$("#addproject-form").submit(function (e) {
    doUpload();
    $("input[type='submit']", this)
        .val("Please Wait...")
        .attr('disabled', 'disabled');

    actionMethod = "POST"
    actionUrl = "/user/addproject"
    var num = parseFloat($('#Goal').val());
    var num2 = parseInt($('#ProjectCreatorId').val());
    let file = $("#ThemeImage")[0].files[0];

    sendData = {
        "ProjectCreatorId": num2,
        "Title": $('#Title').val(),
        "Description": $('#Description').val(),
        "Category": $('#Category').val(),
        "EndDate": $('#EndDate').val(),
        "Goal": num,
        "ThemeImage": file.name
    }


    $.ajax({
        url: actionUrl,
        dataType: 'json',
        type: actionMethod,
        data: JSON.stringify(sendData),

        contentType: 'application/json',
        processData: false,
        success: function (data, textStatus, jQxhr) {
            window.open("/HOME/AddPackage?ProjectId=" + data[0] + "&userId=" + data[1], "_self")
        },
        error: function (jqXhr, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
    e.preventDefault();
});

$("#addpackage-form").submit(function (e) {
    actionMethod = "POST"
    actionUrl = "/user/addpackage"
    var num = parseFloat($('#Amount').val());
    var num2 = parseInt($('#ProjectId').val());
    sendData = {
        "ProjectId": num2,
        "Amount": num,
        "Description": $('#Description').val(),
        "Reward": $('#Reward').val(),
    }

    $.ajax({
        url: actionUrl,
        dataType: 'json',
        type: actionMethod,
        data: JSON.stringify(sendData),

        contentType: 'application/json',
        processData: false,
        success: function (data, textStatus, jQxhr) {
            $('#responseDiv').html("Your Package Added Succesfully <br> Add a new one or return Home ");
        },
        error: function (jqXhr, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
    e.preventDefault();
});

function submitPackageFund(backerId, projId, pckgId) {
    actionMethod = "POST"
    actionUrl = "/user/addfund"
    sendData = {
        "ProjectId": projId,
        "BackerId": backerId,
        "PackageId": pckgId
    }

    $.ajax({
        url: actionUrl,
        dataType: 'json',
        type: actionMethod,
        data: JSON.stringify(sendData),

        contentType: 'application/json',
        processData: false,
        success: function (data, textStatus, jQxhr) {

            $('#responseDiv').html("Thank you for your support! <br> Your Fund Added Succesfully");
            setTimeout(function () {
                window.location.href = "/HOME/BackerIndex?userId=" + backerId
            }, 2000);
            //window.open("/HOME/BackerIndex?userId=" + userId, "_self")
        },
        error: function (jqXhr, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}


function Search(id) {
    var title = document.getElementById("title").value;
    window.open("/HOME/Explore?userId=" + id + "&title=" + title, "_self")
}

function AllCat(id) {
    window.open("/HOME/Explore?userId=" + id, "_self")
}

function Category(id, cat) {
    window.open("/HOME/Explore?userId=" + id + "&category=" + cat, "_self")
}

function Previous(id, page) {
    var btn = document.getElementById("prev-btn");
    var pageNumber = page;
    if (pageNumber == 1) {
        btn.disabled = true;
    } else {
        pageNumber -= 1;
        window.open("/HOME/Explore?userId=" + id + "&pageNumber=" + pageNumber, "_self")
    }
}
function CurrPage(id, page) {
    window.open("/HOME/Explore?userId=" + id + "&pageNumber=" + page, "_self")
}
function Next(id, page, last) {
    var btn = document.getElementById("next-btn");
    var pageNumber = page;
    var lastPage = last;
    if (lastPage == 0) {
        pageNumber += 1;
        window.open("/HOME/Explore?userId=" + id + "&pageNumber=" + pageNumber, "_self")
    } else {
        btn.disabled = true;
    }
}


$("#login-form").submit(function (e) {
    actionMethod = "POST"
    if ($('#Role').val() == 'ProjectCreator') {
        actionUrl = "/user/loginprojectcreator"
        sendData = {
            "Username": $('#Username').val(),
            "Password": $('#Password').val(),
            "Role": $('#Role').val()
        }
    } else {
        actionUrl = "/user/loginbacker"
        sendData = {
            "Username": $('#Username').val(),
            "Password": $('#Password').val(),
            "Role": $('#Role').val()
        }
    }

    $.ajax({
        url: actionUrl,
        dataType: 'json',
        type: actionMethod,
        data: JSON.stringify(sendData),


        contentType: 'application/json',
        processData: false,
        success: function (data, textStatus, jQxhr) {
            if (typeof data === 'undefined') {
                $('#responseDiv').html("The user does not exist");
            } else {
                role = data["role"]
                userId = data["id"]

                if (role == 'ProjectCreator') {
                    window.open("/HOME/ProjectCreatorIndex?userId=" + userId, "_self")
                } else {
                    window.open("/HOME/BackerIndex?userId=" + userId, "_self")
                }
            }
        },
        error: function (jqXhr, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
    e.preventDefault();
});

$("#addmedia-form").submit(function (e) {
    $("input[type='submit']", this)
        .val("Please Wait...")
        .attr('disabled', 'disabled');

    actionMethod = "POST"
    actionUrl = "/project/addmedia"
    var num2 = parseInt($('#ProjectId').val());
    sendData = {
        "ProjectId": num2,
        "Type": $('#Type').val(),
        "URL": $('#URL').val()
    }

    $.ajax({
        url: actionUrl,
        dataType: 'json',
        type: actionMethod,
        data: JSON.stringify(sendData),

        contentType: 'application/json',
        processData: false,
        success: function (data, textStatus, jQxhr) {
            window.open("/HOME/ProjectCreatorProjectView?projectId=" + data[0] + "&userId=" + data[1], "_self")
        },
        error: function (jqXhr, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
    e.preventDefault();
});

$("#addstatus-form").submit(function (e) {
    $("input[type='submit']", this)
        .val("Please Wait...")
        .attr('disabled', 'disabled');

    actionMethod = "POST"
    actionUrl = "/project/addstatus"
    var num2 = parseInt($('#ProjectId').val());
    sendData = {
        "ProjectId": num2,
        "Title": $('#Title').val()
    }

    $.ajax({
        url: actionUrl,
        dataType: 'json',
        type: actionMethod,
        data: JSON.stringify(sendData),

        contentType: 'application/json',
        processData: false,
        success: function (data, textStatus, jQxhr) {
            window.open("/HOME/ProjectCreatorProjectView?projectId=" + data[0] + "&userId=" + data[1], "_self")
        },
        error: function (jqXhr, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
    e.preventDefault();
});

$("#editproject-form").submit(function (e) {

    let file = $("#ThemeImage")[0].files[0];
    var filename = " ";
    if (file) {
        doUpload();
        filename = file.name;
    }
    $("input[type='submit']", this)
        .val("Please Wait...")
        .attr('disabled', 'disabled');

    actionMethod = "POST"
    actionUrl = "/user/editproject"
    var num = parseFloat($('#Goal').val());
    var num2 = parseInt($('#ProjectCreatorId').val());
    var num3 = parseInt($('#ProjectId').val());

    sendData = {
        "ProjectCreatorId": num2,
        "Title": $('#Title').val(),
        "Description": $('#Description').val(),
        "Category": $('#Category').val(),
        "EndDate": $('#EndDate').val(),
        "Goal": num,
        "ThemeImage": filename,
        "projId": num3
    }

    $.ajax({
        url: actionUrl,
        dataType: 'json',
        type: actionMethod,
        data: JSON.stringify(sendData),

        contentType: 'application/json',
        processData: false,
        success: function (data, textStatus, jQxhr) {
            $('#responseDiv').html("Your Project Update was Succesfull");
            window.open("/HOME/ProjectCreatorIndex?userId=" + num2, "_self")
        },
        error: function (jqXhr, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
    e.preventDefault();
});



//It will used for file uploads, sends data using [FromForm] =default
function doUpload() {

    actionMethod = "POST"
    actionUrl = "/user/CreateUsingAjaX"

    var formData = new FormData();
    for (var i = 0; i < $('#ThemeImage').length; i++) {
        formData.append("ThemeImage", $('#ThemeImage')[0].files[i]);
    }

    //alert(JSON.stringify(Object.fromEntries(formData)))

    $.ajax({
        url: actionUrl,
        dataType: 'json',

        type: actionMethod,
        data: formData,

        contentType: false,  // important when sending formData
        processData: false,
        success: function (data) {
            $('#responseDiv').html("The update has been made successfully " + data["returnValue"]);

        },
        error: function (jqXhr, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });


}