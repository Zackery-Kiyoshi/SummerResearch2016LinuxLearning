from django.db import models

# Create your models here.

from django.contrib.auth.models import User
     
class UserProfile(models.Model):
    user = models.OneToOneField(User,
                                unique=True,
                                verbose_name=('user'),
                                related_name='my_profile')
    #url = models.URLField()
    #webLevel = models.IntegerField()
    home_address = models.TextField()
    webLevel = models.IntegerField(default=1)
    unityLevel = models.IntegerField(default=1)
    f = "FUCK"
    
User.profile = property(lambda u: UserProfile.objects.get_or_create(user=u)[0])