
$(document).ready(function () {
    $(function () {
        $(document).on('click', '#btnSave', function (event) {
            event.preventDefault();
            const form = $('#form-comment-add');
            const actionUrl = form.attr('action');
            const dataToSend = form.serialize();
            $.post(actionUrl, dataToSend).done(function (data) {
                const commentAddAjaxModel = jQuery.parseJSON(data);
                console.log(commentAddAjaxModel);
                const newFormBody = $('.form-card', commentAddAjaxModel.CommentAddPartial);
                const cardBody = $('.form-card');
                cardBody.replaceWith(newFormBody);
                const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                if (isValid) {
                    const newSingleComment = `
            <div class="be-comment">
                <div class="be-img-comment">
                    <a href="blog-detail-2.html">
                        <img src="https://bootdey.com/img/Content/avatar/avatar1.png" alt="" class="be-ava-comment">
                    </a>
                </div>
                <div class="be-comment-content">

                    <span class="be-comment-name">
                        <p >${commentAddAjaxModel.CommentDto.Comment.CreatedByName}</p>
                    </span>
                    <p class="be-comment-text">
                        ${commentAddAjaxModel.CommentDto.Comment.Text}
                    </p>
                </div>
            </div>`;
                    const newSingleCommentObject = $(newSingleComment);
                    newSingleCommentObject.hide();
                    $('#comments').append(newSingleCommentObject);
                    newSingleCommentObject.fadeIn(2000);
                    toastr.success(`Yorum onaylandıktan sonra aktif olacaktır.`);
                    $('#btnSave').prop('disabled', true);
                    setTimeout(function() {
                        $('#btnSave').prop('disabled', false);
                    },15000);
                }
                else {
                    let summaryText = "";
                    $('#validation-summary > ul > li').each(function () {
                        let text = $(this).text();
                        summaryText += `*${text}\n`;
                    });
                    toastr.warning(summaryText);
                }
            }).fail(function (error) {
                console.error(error);
            });
        });
    });
});