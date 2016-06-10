from django.contrib import admin

# Register your models here.

from loginSystem.models import UserProfile

admin.site.register(UserProfile)