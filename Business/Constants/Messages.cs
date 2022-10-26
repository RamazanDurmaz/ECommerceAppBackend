using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Constants
{
    public static class Messages
    {
        public static string ProductAdded = "Ürün Başarıyla Eklendi";
        public static string ProductDeleted = "Ürün Başarıyla Silindi";
        public static string ProductUpdated = "Ürün Başarıyla Güncelendi";
        public static string UnitPriceInvalid = "Geçersiz Ürün Fiyatı";
        public static string ProductNameInvalid = "Ürün Adı Geçersiz";

        public static string UserNotFound = "Kullanıcı Bulunamadı";
        public static string PasswordError = "Şifre Hatalı";
        public static string SuccessfulLogin = "Sisteme Giriş Başarılı";
        public static string UserAlreadyExists = "Bu Kullanıcı Zaten Mevcut";

        public static string UserRegistered = "Kullanıcı Başarıyla kaydedildi";

        public static string AccessTokenCreated = "Access Token Başarıyla Oluşturuldu";

        public static string ProductCountOfCategoryError = "Bir Kategoride En Fazla 10 Ürün Olabilir";

        public static string ProductNameAlreadyExists = "Bu İsimde Zaten Bir Ürün Mevcut";

        public static string CategoryLimitExceded = "Kategori Limiti Aşıldığı için Yeni Ürün Eklenemiyor";

        public static string AuthorizationDenied = "Yetkiniz Bulunmamaktadır";

    }
}
