# -*- coding: utf-8 -*-
# Generated by Django 1.9.7 on 2016-06-07 19:07
from __future__ import unicode_literals

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('loginSystem', '0001_initial'),
    ]

    operations = [
        migrations.AddField(
            model_name='userprofile',
            name='unityLevel',
            field=models.IntegerField(default=1),
        ),
        migrations.AlterField(
            model_name='userprofile',
            name='webLevel',
            field=models.IntegerField(default=1),
        ),
    ]
