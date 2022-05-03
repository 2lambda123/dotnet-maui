package com.microsoft.maui.glide;

import android.graphics.drawable.Drawable;
import android.os.Handler;
import android.os.Looper;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

import com.bumptech.glide.RequestManager;
import com.bumptech.glide.request.target.CustomTarget;
import com.bumptech.glide.request.transition.Transition;
import com.microsoft.maui.ImageLoaderCallback;

public class MauiCustomTarget extends CustomTarget<Drawable> {
    private final ImageLoaderCallback callback;
    private final RequestManager requestManager;
    private boolean completed;

    public MauiCustomTarget(ImageLoaderCallback callback, RequestManager requestManager) {
        this.completed = false;
        this.callback = callback;
        this.requestManager = requestManager;
    }

    @Override
    public void onLoadFailed(@Nullable Drawable errorDrawable) {
        if (completed)
            return;
        this.completed = true;

        new Handler(Looper.getMainLooper())
            .post(() -> requestManager.clear(MauiCustomTarget.this));

        callback.onComplete(false, errorDrawable, null);
    }

    @Override
    public void onResourceReady(@NonNull Drawable resource, @Nullable Transition<? super Drawable> transition) {
        if (completed)
            return;
        this.completed = true;

        callback.onComplete(
            true,
            resource,
            () -> new Handler(Looper.getMainLooper())
                .post(() -> requestManager.clear(MauiCustomTarget.this)));
    }

    @Override
    public void onLoadCleared(@Nullable Drawable placeholder) {
    }
}

