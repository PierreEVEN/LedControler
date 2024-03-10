package even.pierre.ledcontroler.ui.tools;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProvider;

import even.pierre.ledcontroler.databinding.FragmentPannerStripBinding;

public class PannerStripFragment extends Fragment {

    private FragmentPannerStripBinding binding;

    public View onCreateView(@NonNull LayoutInflater inflater,
                             ViewGroup container, Bundle savedInstanceState) {
        UniformStripViewModel slideshowViewModel =
                new ViewModelProvider(this).get(UniformStripViewModel.class);

        binding = FragmentPannerStripBinding.inflate(inflater, container, false);
        View root = binding.getRoot();

        final TextView textView = binding.textStatic;
        slideshowViewModel.getText().observe(getViewLifecycleOwner(), textView::setText);
        return root;
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        binding = null;
    }
}