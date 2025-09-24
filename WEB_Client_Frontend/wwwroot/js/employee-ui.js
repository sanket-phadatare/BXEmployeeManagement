// employee-ui.js
let currentPage = 1;
let pageSize = 5;
let allEmployees = [];
let currentSortColumn = null;
let sortAscending = true;

$(document).ready(function () {
    fetchEmployees();

    $("#searchBox").on("input", function () {
        renderTable();
    });

    $("#nextPage").click(function () {
        const maxPage = Math.ceil(filteredEmployees().length / pageSize);
        if (currentPage < maxPage) {
            currentPage++;
            renderTable();
        }
    });

    $("#prevPage").click(function () {
        if (currentPage > 1) {
            currentPage--;
            renderTable();
        }
    });

    $("#employeeTableBody").on("click", ".edit-btn", function () {
        const id = $(this).data("id");
        const emp = allEmployees.find(e => e.id === id);
        if (emp) {
            fillForm(emp);
            $("#editModal").modal("show");
        }
    });

    $("#editDOB").on("change", function () {
        const dob = new Date($(this).val());
        const today = new Date();
        let age = today.getFullYear() - dob.getFullYear();
        if (dob > new Date(today.setFullYear(today.getFullYear() - age))) age--;
        $("#editAge").val(age);
    });

    $("#editForm").submit(function (e) {
        e.preventDefault();
        const id = $("#editId").val();
        const updatedEmp = {
            Id: id,
            Name: $("#editName").val(),
            Designation: $("#editDesignation").val(),
            DateOfBirth: $("#editDOB").val(),
            Salary: $("#editSalary").val(),
            Gender: $("#editGender").val(),
            State: $("#editState").val(),
            Age: $("#editAge").val(),
        };

        $.ajax({
            url: /employee/update / ${ id },
            method: 'PUT',
            contentType: "application/json",
            data: JSON.stringify(updatedEmp),
            success: function () {
                fetchEmployees();
                $('#editModal').modal('hide');
            }
        });
});

$.ajax({
    url: '/employee/deleteSelected',
    type: 'POST',
    contentType: 'application/json', // Ensure this is set to JSON
    data: JSON.stringify({ ids: selectedIds }), // Ensure this sends the correct format
    success: function (result) {
        if (result.success) {
            location.reload();
        } else {
            alert(result.message || 'Error deleting selected employees');
        }
    },
    error: function () {
        alert('Server error while deleting selected employees');
    }
});





$("#selectAll").click(function () {
    $("input[name='rowCheckbox']").prop("checked", this.checked);
});

$("#clearForm").click(function () {
    $("#editForm")[0].reset();
});

$("th.sortable").click(function () {
    const column = $(this).data("column");
    if (currentSortColumn === column) {
        sortAscending = !sortAscending;
    } else {
        currentSortColumn = column;
        sortAscending = true;
    }
    renderTable();
});
});

function fetchEmployees() {
    $.get("/employee/getall", function (data) {
        allEmployees = data;
        renderTable();
    });
}

function filteredEmployees() {
    let search = $("#searchBox").val().toLowerCase();
    let filtered = allEmployees.filter(e => e.name.toLowerCase().includes(search));
    if (currentSortColumn) {
        filtered.sort((a, b) => {
            let valA = a[currentSortColumn];
            let valB = b[currentSortColumn];
            if (typeof valA === 'string') valA = valA.toLowerCase();
            if (typeof valB === 'string') valB = valB.toLowerCase();
            return sortAscending ? valA > valB ? 1 : -1 : valA < valB ? 1 : -1;
        });
    }
    return filtered;
}

function renderTable() {
    let filtered = filteredEmployees();
    let pageData = filtered.slice((currentPage - 1) * pageSize, currentPage * pageSize);
    let rows = "";
    let totalSalary = 0;
    pageData.forEach(emp => {
        totalSalary += parseFloat(emp.salary);
        rows +=
            <tr>
                <td><input type="checkbox" name="rowCheckbox" value="${emp.id}" /></td>
                <td><a href="#" class="edit-btn" data-id="${emp.id}">${emp.name}</a></td>
                <td>${emp.designation}</td>
                <td>${emp.age}</td>
                <td>${emp.salary}</td>
                <td>${emp.gender}</td>
                <td>${emp.state}</td>
                <td>
                    <button class="btn btn-sm btn-danger delete-btn" data-id="${emp.id}">Delete</button>
                </td>
            </tr>;
    });
    $("#employeeTableBody").html(rows);
    $("#totalSalary").text(totalSalary.toFixed(2));

    $('.deleteBtn').click(function () {
        var employeeId = $(this).data('id');

        if (confirm('Are you sure you want to delete this employee?')) {
            $.ajax({
                url: '/employee/delete/' + employeeId,  // Use POST instead of DELETE (for compatibility)
                type: 'POST',
                success: function (result) {
                    location.reload();
                },
                error: function (error) {
                    alert('Error deleting employee');
                }
            });
        }
    });

}

function fillForm(emp) {
    $("#editId").val(emp.id);
    $("#editName").val(emp.name);
    $("#editDesignation").val(emp.designation);
    $("#editDOB").val(emp.dateOfBirth.substring(0, 10));
    $("#editSalary").val(emp.salary);
    $("#editGender").val(emp.gender);
    $("#editState").val(emp.state);
    $("#editAge").val(emp.age);
}