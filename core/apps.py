from django.apps import AppConfig
from django.conf import settings as s

import os
import urllib

class CoreConfig(AppConfig):
    name = 'core'
    verbose_name = 'Open ONI Core'

    def ready(self):
        # Define internal settings derived from local settings
            # BASE_URL-dependent settings
        url = urllib.parse.urlparse(s.BASE_URL)

        s.ALLOWED_HOSTS = [url.netloc]

        # HTTPS settings
        if url.scheme == 'https':
            s.CSRF_COOKIE_SECURE = True
            s.SESSION_COOKIE_SECURE = True

            s.SECURE_HSTS_INCLUDE_SUBDOMAINS = s.SECURE_HSTS_SECONDS > 0
            s.SECURE_HSTS_PRELOAD = s.SECURE_HSTS_SECONDS > 0
            s.SECURE_SSL_REDIRECT = s.SECURE_HSTS_SECONDS > 0
            # / BASE_URL-dependent settings

        s.SOLR = s.SOLR_BASE_URL + '/solr/openoni'

            # STORAGE-dependent settings
        s.BATCH_STORAGE = os.path.join(s.STORAGE, 'batches')
        s.COORD_STORAGE = os.path.join(s.STORAGE, 'word_coordinates')
        s.OCR_DUMP_STORAGE = os.path.join(s.STORAGE, 'ocr')
        s.TEMP_TEST_DATA = os.path.join(s.STORAGE, 'temp_test_data')
            # / STORAGE-dependent settings

        return
