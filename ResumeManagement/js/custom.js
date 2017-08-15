$(function () {
	
	// register
	$('#register-form').submit(function (event) {
		var password = $('#password').val();
		var confirm = $('#confirm').val();
		
		if (password !== confirm) {
			$('.trigger-register-message').removeClass('hide');
			$('.trigger-register-message').html("Password and confirm fields need to be identical.");
			return false;
		}
		
		$('.trigger-register-message').addClass('hide');
		
		return true;
	});
	
	// add/edit item from items
	$('.trigger-item-back').bind('click', function (event) {
	    location.href = $(this).data('link');
	});
	
    // delete item (resume/cvletter/package) from items
	$('.trigger-item-delete').bind('click', function (event) {
		if (confirm('Are you sure to delete this item?')) {
		    $(this).closest('form').submit();
		}
	});
	
	// click item (resume/cvletter/package) in items
	$('.panel .row').bind('click', function (event) {
	    $tr = $(this).closest('tr');
	    location.href = $tr.data('link') + $tr.data('id');
	});
});