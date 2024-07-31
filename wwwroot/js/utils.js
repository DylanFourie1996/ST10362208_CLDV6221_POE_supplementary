<script>
    $(document).ready(function () {
        updateCartItemCount();

    // Function to update cart item count
    function updateCartItemCount() {
        $.ajax({
            url: '@Url.Action("GetCartItemCount", "Cart")',
            type: 'GET',
            success: function (data) {
                $('#cartItemCount').text(data);
            },
            error: function () {
                console.log('Error retrieving cart item count.');
            }
        });
        }
    });
</script>